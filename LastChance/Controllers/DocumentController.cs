using DocumentFormat.OpenXml.Spreadsheet;
using LastChance.Models.DocumentData;
using LastChance.Models.LoginData;
using LastChance.Models.ViewModels;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LastChance.Controllers
{
    public class DocumentController : Controller
    {
        private static Microsoft.Office.Interop.Excel.Workbook MyBook = null;
        private static Microsoft.Office.Interop.Excel.Application MyApp = null;
        private static Microsoft.Office.Interop.Excel.Worksheet MyWorkSheet = null;

        // GET: Document
        public ActionResult Index()
        {
            var sth = AppDomain.CurrentDomain;
            
            return View();
        }
        
        public ActionResult RegisterDocument()
        {
             using (var db = new DocumentDbContext())
            {
                ViewData["Lines"] = new SelectList(db.Lines.ToList(),"LineId","LineName") ;
                ViewData["User"] = new SelectList(db.Users.ToList(), "UserId", "UserName");
                ViewData["Operation"] = new SelectList(db.Operations.ToList(), "OperationId", "OperationNumber");
                ViewData["DocumentVariant"] = new SelectList(db.DocumentTypes.ToList(), "DocumentTypeId", "Description");
                var authorSelect = from us in db.UserAccept
                                   join su in db.Users on us.AcceptedUserId equals su.AcceptUser.AcceptedUserId
                                   select us;
                ViewData["AcceptedUser"] = new SelectList(authorSelect.ToList(), "AcceptedUserId", "User.Email");
                
            }
            return View();
        }
       
        public JsonResult GetOperations(string id)
        {
            int parse = int.Parse(id);
            List<Operation> listOfOperations = new List<Operation>();
            using (var db = new DocumentDbContext())
            {
                foreach (var item in db.Operations.Where(x=>x.OperationLine.LineId==parse).ToList())
                {
                    switch (id)
                    {
                        case "4":
                            listOfOperations.Add(item);
                            break;
                        case "5":
                            listOfOperations.Add(item);
                            break;
                        case "6":
                            listOfOperations.Add(item);
                            break;
                        case "7":
                            listOfOperations.Add(item);
                            break;
                        case "8":
                            listOfOperations.Add(item);
                            break;
                        default:
                            break;
                    }
                }
            }
            return Json(new SelectList(listOfOperations, "OperationId", "OperationNumber"));
         
        }

        private byte[] FillDocument(byte [] file, string title, string description, string author,
                                    string documentType,string line)
        {
            byte[] newFile = file;
            

            return file;
        }
        [HttpPost]
        public ActionResult RegisterDocument(Document doc, HttpPostedFileBase file, string Title, int LineId,
                                            int OperationId, int DocumentTypeId, int AcceptedUserId)
        {
            //stworzenie sciezki do zapisania pliku
            //index idLini-idOperarcji-idInstr
            string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string pathDownload = Path.Combine(pathUser, "Downloads");
            string title = Title + DateTime.Now;
            title = title.Replace("-", "");
            title = title.Replace(":", "");
            string title2 = LineId.ToString() + ".pdf";
            string EndPath = Path.Combine(pathDownload, title);
            string EndPathPDF = Path.Combine(pathDownload, title2);

            //stworzenie tablicy wielkosci 
            doc.DocumentFile = new byte[file.InputStream.Length];
            file.InputStream.Read(doc.DocumentFile, 0, doc.DocumentFile.Length);
            System.IO.File.WriteAllBytes(EndPath, doc.DocumentFile);

            MyApp = new Microsoft.Office.Interop.Excel.Application();
            MyApp.Visible = false;
            MyBook = MyApp.Workbooks.Open(EndPath);
            MyWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)MyBook.Sheets[1];
            using (var db = new DocumentDbContext())
            {
                MyWorkSheet.Cells[3, 3] = db.Lines.Where(x => x.LineId == LineId).First().LineName;
                MyWorkSheet.Cells[2, 5] = Title;
                MyWorkSheet.Cells[2, 13] = 1;
                MyWorkSheet.Cells[4, 3] = db.Operations.Where(x => x.OperationId == OperationId).First().OperationNumber.ToString();
                int NewId;
                Document temp = db.Documents.FirstOrDefault();
                if (temp == null)
                {
                    doc.DocumentId = 1; ;
                    NewId = 1;
                }
                else {
                    NewId = db.Documents.Max(x => x.DocumentId);
                    NewId++;
                }
                    doc.DocumentId = NewId;

                    UserAccount tempAuthor;
                    User authorUser;

                    var user = (string)Session["UserName"];
                    string index = LineId.ToString() + "-" + OperationId.ToString() + "-" + DocumentTypeId.ToString()
                         + "-" + NewId;
                    MyWorkSheet.Cells[5, 5] = db.DocumentTypes.Where(x => x.DocumentTypeId == DocumentTypeId).First().Description;
                    MyWorkSheet.Cells[2, 3] = index;
                    
                    using (var db1 = new AccountDbContext())
                    {
                        tempAuthor = db1.userAccount.Where(x => x.UserName == user).First();
                        authorUser = db.Users.Where(x => x.LastName == tempAuthor.LastName).First();
                        MyWorkSheet.Cells[4, 13] = authorUser.FirstName + " " + authorUser.LastName;
                    }
                    Microsoft.Office.Interop.Excel.Range range = (Microsoft.Office.Interop.Excel.Range)MyWorkSheet.Cells[1, 17];
                    MyWorkSheet.PageSetup.CenterFooter = "Dokument wygenerowany przez program DocumentManager";
                    string cellValue = range.Value.ToString();
                    MyApp.Save();
                    MyBook.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, EndPathPDF);
                    MyBook.Close();
                    byte[] f1 = new byte[15728640];
                    f1 = System.IO.File.ReadAllBytes(EndPath);
                    byte[] fPDF = new byte[15728640];
                    fPDF = System.IO.File.ReadAllBytes(EndPathPDF);
                    if (cellValue != "INSTRDOC")    
                    {
                        return RedirectToAction("ErrorTemplate");
                    }
                    else
                    {
                        //
                        doc.DocumentFileExcel = f1;
                        doc.DocumentFile = fPDF;
                        doc.Status = db.Status.Where(x => x.StatusId == 1).First();
                        doc.Version = 1;
                        doc.Title = Title;
                        doc.LineId = LineId;
                        //nadanie indexu  IdLini-IdOperacji-IdTypuInstruckji-IdPierwszejInstrukcji
                        doc.Index = index;
                        doc.DateOfAdding = DateTime.Now;
                        doc.OperationId = OperationId;
                        doc.DocumentTypeId = DocumentTypeId;
                        doc.AcceptedUserId = AcceptedUserId;
                        doc.AuthorUserId = authorUser.UserId;
                        db.Documents.Add(doc);
                        db.SaveChanges();

                    if (System.IO.File.Exists(EndPathPDF))
                    {
                        System.IO.File.Delete(EndPathPDF);
                    }
                    if (System.IO.File.Exists(EndPath))
                    {
                        System.IO.File.Delete(EndPath);
                    }
                }
                
            }
            return RedirectToAction("RegisterDocument");
        }
        
        public ActionResult ErrorTemplate()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            if (file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                file.SaveAs(path);
            }

            return RedirectToAction("Index");
        }
    }
}
