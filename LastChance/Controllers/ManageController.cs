using LastChance.Models.DocumentData;
using LastChance.Models.ViewModels;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Web.Mvc;
using System;
using Microsoft.Office.Interop.Excel;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.Diagnostics;
using LastChance.Models.PdfSharp;
using System.Web;
using LastChance.Models.LoginData;

namespace LastChance.Controllers
{
    public class ManageController : Controller
    {
        private static Microsoft.Office.Interop.Excel.Workbook MyBook = null;
        private static Microsoft.Office.Interop.Excel.Application MyApp = null;
        private static Microsoft.Office.Interop.Excel.Worksheet MyWorkSheet = null;

        public ActionResult DeleteDocument()
        {
            List<Document> listofDocument = new List<Document>();
            using (var db = new DocumentDbContext())
            {
                foreach (var item in db.Documents.Where(x => x.StatusId == 4 || x.StatusId==7).ToList())
                {
                    Document doc = new Document();
                    doc.DocumentId = item.DocumentId;
                    doc.Version = item.Version;
                    doc.Title = item.Title;
                    doc.Line = item.Line;
                    doc.Operation = item.Operation;
                    doc.DateOfPublic = item.DateOfPublic;
                    doc.DocumentFile = item.DocumentFile;
                    doc.Index = item.Index;
                    listofDocument.Add(doc);
                }
            }
            return View(listofDocument);

        }
        public ActionResult UpdateDocument()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UpdateDocument(string idDoc)
        {
            return RedirectToAction("ShowDoc","Manage", new { idDoc = idDoc });
        }
        private Document GetNewOne(DocumentDbContext db, string idDoc)
        {
            Document doc = new Document();
            var item = db.Documents.Where(x => x.Index == idDoc).First();
            doc.DocumentId = item.DocumentId;
            doc.DocumentType = item.DocumentType;
            doc.DocumentFile = item.DocumentFile;
            doc.DocumentType = item.DocumentType;
            doc.DocumentFileExcel = item.DocumentFileExcel;
            doc.Title = item.Title;
            doc.Index = item.Index;
            doc.Version = item.Version;
            doc.Line = item.Line;
            doc.Operation = item.Operation;
            doc.LineId = item.LineId;
            doc.OperationId = item.OperationId;
            doc.AuthorUserId = item.AuthorUserId;
            doc.DocumentTypeId = item.DocumentTypeId;
            return doc;
        }
        private Document GetOldOne(DocumentDbContext db, string idDoc)
        {
            Document doc = new Document();
            var item = db.Documents.Where(x => x.Index == idDoc).ToList();
            int myMax = item.Max(x => x.Version);
            var myOldDoc = item.Where(x => x.Version == myMax).First();
            doc.DocumentId = myOldDoc.DocumentId;
            doc.DocumentType = myOldDoc.DocumentType;
            doc.DocumentFile = myOldDoc.DocumentFile;
            doc.DocumentType = myOldDoc.DocumentType;
            doc.DocumentFileExcel = myOldDoc.DocumentFileExcel;
            doc.Title = myOldDoc.Title;
            doc.Index = myOldDoc.Index;
            doc.Version = myOldDoc.Version;
            doc.Line = myOldDoc.Line;
            doc.Operation = myOldDoc.Operation;
            doc.LineId = myOldDoc.LineId;
            doc.OperationId = myOldDoc.OperationId;
            doc.AuthorUserId = myOldDoc.AuthorUserId;
            doc.DocumentTypeId = myOldDoc.DocumentTypeId;
            return doc;
        }
        public ActionResult DocumentDoesntExist()
        {
            return View();
        }
        public ActionResult ShowDoc(string idDoc)
        {
            Document doc = new Document();
            List<AcceptedUser> listOfAccept = new List<AcceptedUser>();
            using (var db = new DocumentDbContext())
            {
                bool exist = db.Documents.ToList().Any(x => x.Index == idDoc);
                if(exist==false)
                {
                    return  View("DocumentDoesntExist");
                }

                doc = GetNewOne(db, idDoc);
                foreach (var item in db.UserAccept.ToList())
                {
                    AcceptedUser temp = new AcceptedUser();
                    temp.AcceptedUserId = item.AcceptedUserId;
                    temp.DoccumentAccepted = item.DoccumentAccepted;
                    temp.User = item.User;
                    listOfAccept.Add(temp);
                }
                ViewData["Lines"] = new SelectList(db.Lines.ToList(), "LineId", "LineName");
                ViewData["User"] = new SelectList(db.Users.ToList(), "UserId", "UserName");
                ViewData["Operation"] = new SelectList(db.Operations.ToList(), "OperationId", "OperationNumber");
                ViewData["DocumentVariant"] = new SelectList(db.DocumentTypes.ToList(), "DocumentTypeId", "Description");
            }
            ViewData["AcceptUser"] = new SelectList(listOfAccept, "AcceptedUserId", "User.Email");

            return View(doc); 
        }
        [HttpPost]
        public ActionResult ShowDoc(HttpPostedFileBase file, string Title,string Index, int AcceptedUserId)
        {
            Document oldDocument;
            Document newDocument;
            using (var db = new DocumentDbContext())
            {
                oldDocument = GetOldOne(db, Index);
                newDocument = GetNewOne(db, Index);
                //stworzenie sciezki do zapisania pliku
                string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string pathDownload = Path.Combine(pathUser, "Downloads");
                string title = Title + DateTime.Now;
                title = title.Replace("-", "");
                title = title.Replace(":", "");
                string title2 = "temp" + ".pdf";
                string EndPath = Path.Combine(pathDownload, title);
                string EndPathPDF = Path.Combine(pathDownload, title2);
                int currentVersion = oldDocument.Version+1;

                //stworzenie tablicy wielkosci 
                newDocument.DocumentFile = new byte[file.InputStream.Length];
                file.InputStream.Read(newDocument.DocumentFile, 0, newDocument.DocumentFile.Length);
                System.IO.File.WriteAllBytes(EndPath, newDocument.DocumentFile);

                MyApp = new Microsoft.Office.Interop.Excel.Application();
                MyApp.Visible = false;
                MyBook = MyApp.Workbooks.Open(EndPath);
                MyWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)MyBook.Sheets[1];
                MyWorkSheet.Cells[2, 3] = oldDocument.Index;
                MyWorkSheet.Cells[3, 3] = db.Lines.Where(x => x.LineId ==oldDocument.LineId).First().LineName;
                MyWorkSheet.Cells[2, 5] = Title;
                MyWorkSheet.Cells[2, 13] = currentVersion;
                MyWorkSheet.Cells[4, 3] = db.Operations.Where(x => x.OperationId ==oldDocument.OperationId).First().OperationNumber.ToString();
                MyWorkSheet.Cells[5, 5] = db.DocumentTypes.Where(x => x.DocumentTypeId == oldDocument.DocumentTypeId).First().Description;

                UserAccount tempAuthor;
                    User authorUser;

                    var user = (string)Session["UserName"];

                    using (var db1 = new AccountDbContext())
                    {
                        tempAuthor = db1.userAccount.Where(x => x.UserName == user).First();
                        authorUser = db.Users.Where(x => x.LastName == tempAuthor.LastName).First();
                        MyWorkSheet.Cells[4, 13] = authorUser.FirstName + " " + authorUser.LastName;
                    }
                    MyApp.Save();
                    MyBook.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, EndPathPDF);
                    MyBook.Close();
                    byte[] f1 = new byte[15728640];
                    f1 = System.IO.File.ReadAllBytes(EndPath);

                    byte[] fPDF = new byte[15728640];
                    fPDF = System.IO.File.ReadAllBytes(EndPathPDF);
                    newDocument.DocumentFileExcel = f1;
                    newDocument.DocumentFile = fPDF;
                    newDocument.Status = db.Status.Where(x => x.StatusId == 1).First();
                    newDocument.Version = currentVersion;
                    newDocument.Title = Title;
                    newDocument.LineId =oldDocument.LineId;
                    newDocument.DateOfAdding = DateTime.Now;
                    newDocument.OperationId = oldDocument.OperationId;
                    newDocument.DocumentTypeId = oldDocument.DocumentTypeId;
                    newDocument.AuthorUserId = authorUser.UserId;
                    newDocument.AcceptedUserId = AcceptedUserId;
                    db.Documents.Add(newDocument);
                    db.SaveChanges();
                if (System.IO.File.Exists(EndPath))
                {
                    System.IO.File.Delete(EndPath);
                }
                if (System.IO.File.Exists(EndPathPDF))
                {
                    System.IO.File.Delete(EndPathPDF);
                }

            }
            
            return RedirectToAction("ShowMessage");
        }
        public ActionResult ShowMessage()
        {
            return View();
        }
        public ActionResult Delete(int id)
        {
            //3 funkcja kierownika
            List<DocumentUserDelVM> listOfUser = new List<DocumentUserDelVM>();
            using (var db = new DocumentDbContext())
            {
                foreach (var item in db.Users.Where(x => x.Function.EmployeeFunctionId == 2).ToList())
                {
                    listOfUser.Add(new DocumentUserDelVM(id,item.UserId,item.FirstName,item.LastName,
                        item.Email,item.Function.EmployeeFunctionId));
                }
                ViewData["myDocId"] = id;
                ViewData["UserDoc"] = new SelectList(listOfUser, "UserId", "LastName");
                
            }
            return View();
        }
        [HttpPost]
        public ActionResult Delete(int UserId, int id)
        {
            //przypisanie osoby odpowiedzialnej za zaakceptowanie usuniecia
            //7 status oczekuje na akceptacje do usuniecia
            using (var db = new DocumentDbContext())
            {
                db.Documents.Where(x => x.DocumentId == id).First().DeleteUser = db.DeleteAuthor.Where(z => z.DeleteUserId == UserId).First();
                db.Documents.Where(x => x.DocumentId == id).First().StatusId = 7;
                db.SaveChanges();
            }
            return RedirectToAction("DeleteDocument");
        }
        
        public ActionResult LineDocs()
        {
            List<Models.DocumentData.Line> listOfLine = new List<Models.DocumentData.Line>();
            using (var db = new DocumentDbContext())
            {
                
                foreach (var item in db.Lines.ToList())
                {
                    Models.DocumentData.Line temp = new Models.DocumentData.Line();
                    temp.LineId = item.LineId;
                    temp.LineName = item.LineName;
                    temp.Operations = item.Operations;
                    listOfLine.Add(temp);
                }

            }
                return View(listOfLine);
        }
        [HttpPost]
        public ActionResult LineDocs(List<Document> listOfLine)
        {
            return View();
        }
        public ActionResult Details(int id)
        {
            List<Document> ListDocToShow = new List<Document>();
            Models.DocumentData.Line line;
            using (var db = new DocumentDbContext())
            {
                line = db.Lines.Where(x => x.LineId == id).First();
                foreach (var item in db.Documents.Where(x=>x.LineId==id && x.StatusId==4 || x.StatusId==7).ToList())
                {
                    Document temp = new Document();
                    temp.AccepedUser = item.AccepedUser;
                    temp.AcceptedUserId = item.AcceptedUserId;
                    temp.AuthorUser = item.AuthorUser;
                    temp.AuthorUserId = item.AuthorUserId;
                    temp.Line = item.Line;
                    temp.Operation = item.Operation;
                    temp.Title = item.Title;
                    temp.Comments = item.Comments;
                    temp.DateOfAcceptance = item.DateOfAcceptance;
                    temp.DateOfAdding = item.DateOfAdding;
                    temp.Index = item.Index;
                    temp.DateOfPublic = item.DateOfPublic;
                    temp.DocumentFile = item.DocumentFile;
                    temp.DocumentFileExcel = item.DocumentFileExcel;
                    temp.DocumentId = item.DocumentId;
                    temp.Version = item.Version;
                    temp.DocumentType =db.DocumentTypes.Where(x=>x.DocumentTypeId==item.DocumentTypeId).First();
                    temp.DocumentTypeId = item.DocumentTypeId;
                    temp.StatusId = item.StatusId;
                    temp.Title = item.Title;
                    ListDocToShow.Add(temp);
                }
            }
            if (ListDocToShow.Count==0)
            {
                return RedirectToAction("NoInformation");
            }
            else
            {
                PdfDocument document = new PdfDocument();

                LayoutHelper helper = new LayoutHelper(document, XUnit.FromCentimeter(2.5), XUnit.FromCentimeter(29.7 - 2.5));
                XUnit left = XUnit.FromCentimeter(2.5);

                const int headerFontSize = 10;
                const int normalFontSize = 10;

                helper.SetWatermark("DocumentManager"+" "+line.LineName+" "+DateTime.Today.ToShortDateString());
                XFont fontHeader = new XFont("Times New Roman", headerFontSize, XFontStyle.BoldItalic);
                XFont fontNormal = new XFont("Times New Roman", normalFontSize, XFontStyle.Regular);
                double countPages = (double)ListDocToShow.Count / 56;
                double countPage = Math.Ceiling(countPages);
                int currentPage = 1;
                for (int lineSheet = 0; lineSheet < ListDocToShow.Count; ++lineSheet)
                {
                    XUnit top;
                    bool isHeader = lineSheet == 0 || lineSheet % 56 == 0;
                    top = helper.GetLinePosition(isHeader ? headerFontSize + 5 : normalFontSize + 2);
                    if (isHeader)
                    {
                        string text ="Strona "+ currentPage.ToString() + "/" + countPage.ToString();
                        helper.Gfx.DrawString(text,
                        fontNormal, XBrushes.Black, left, top, XStringFormats.TopLeft);
                        currentPage++;
                        top = helper.GetLinePosition(normalFontSize + 2);
                        helper.Gfx.DrawString("Index " + "Typ dokumentu " + "Opis" + "Operacja"+ "Wersja" ,
                         fontHeader, XBrushes.Black, left, top, XStringFormats.TopLeft);
                        top = helper.GetLinePosition(normalFontSize + 2);

                        helper.Gfx.DrawString((ListDocToShow[lineSheet].Index + " " +
                        ListDocToShow[lineSheet].DocumentType.Description.ToString())+" "+ ListDocToShow[lineSheet].Title+" " +
                        ListDocToShow[lineSheet].Operation.OperationNumber + " " + ListDocToShow[lineSheet].Version.ToString(),
                        fontNormal, XBrushes.Black, left, top, XStringFormats.TopLeft);
                    }

                    else
                    {
                        helper.Gfx.DrawString((ListDocToShow[lineSheet].Index + " " +
                        ListDocToShow[lineSheet].DocumentType.Description.ToString()) + " " + ListDocToShow[lineSheet].Title + " " +
                        ListDocToShow[lineSheet].Operation.OperationNumber + " " + ListDocToShow[lineSheet].Version.ToString(),
                        fontNormal, XBrushes.Black, left, top, XStringFormats.TopLeft);
                    }
                }
                //tworzenie ścieżki zapisu pliku
                string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string pathDownload = Path.Combine(pathUser, "Downloads");
                string name = line.LineName + DateTime.Now.ToString() + ".pdf";
                name= name.Replace("-", "");
                name= name.Replace(":", "");
                string path = Path.Combine(pathDownload, name);
                document.Save(path);
                //Uruchomienie pliku
                Process.Start(path);

                return View(ListDocToShow);
            }
        }
       
        public string NoInformation()
        {
            return "Brak dokumentów do Wyświetlenia";
        }
        public ActionResult GetPDF(int docId)
        {
            string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string pathDownload = Path.Combine(pathUser, "Downloads");
            string title;
            byte[] temp;
            string EndPath;
            using (var db = new DocumentDbContext())
            {

                temp = new byte[db.Documents.Where(x=>x.DocumentId== docId).First().DocumentFile.Length];
                temp = db.Documents.Where(x=>x.DocumentId==docId).First().DocumentFile;
                title = db.Documents.Where(x => x.DocumentId == docId).First().Title + ".pdf";
                EndPath = Path.Combine(pathDownload, title);
                System.IO.File.WriteAllBytes(EndPath, temp);

            }
            System.Diagnostics.Process.Start(EndPath);
            return RedirectToAction("Index");
        }
        public ActionResult GetExcel(int docId)
        {
            string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string pathDownload = Path.Combine(pathUser, "Downloads");
            string title;
            byte[] temp;
            string EndPath;
            using (var db = new DocumentDbContext())
            {

                temp = new byte[db.Documents.Where(x => x.DocumentId == docId).First().DocumentFileExcel.Length];
                temp = db.Documents.Where(x => x.DocumentId == docId).First().DocumentFileExcel;
                title = db.Documents.Where(x => x.DocumentId == docId).First().Title + ".xls";
                EndPath = Path.Combine(pathDownload, title);
                System.IO.File.WriteAllBytes(EndPath, temp);

            }
            System.Diagnostics.Process.Start(EndPath);
            return RedirectToAction("Index");
        }
        // GET: Manage
        public ActionResult Index()
        {
            List<DocumentAndTypeVM> ListDocToShow = new List<DocumentAndTypeVM>();
            using (var db = new DocumentDbContext())
            {
                foreach (var item in db.Documents.ToList())
                {
                  
                    DocumentAndTypeVM temp = new DocumentAndTypeVM();
                    temp.Title = item.Title;
                    temp.Version = item.Version;
                    temp.DocumentFile = item.DocumentFile;
                    temp.DocumentFileExcel = item.DocumentFileExcel;
                    temp.DocumentId = item.DocumentId;
                    temp.Index = item.Index;
                    temp.DescriptionOp = db.Operations.Where(x => x.OperationId == item.OperationId).First().Description;
                    temp.OperationNumber = db.Operations.Where(x => x.OperationId == item.OperationId).First().OperationNumber;
                    temp.DocumentTypeDescription = db.DocumentTypes.Where(x => x.DocumentTypeId == item.DocumentTypeId).First().Description;
                    temp.LineName = db.Lines.Where(x => x.LineId == item.LineId).First().LineName;
                    ListDocToShow.Add(temp);
                }
                ViewData["Lines"] = new SelectList(ListDocToShow,"LineName","LineName");
                    
                ViewData["Operation"] = new SelectList(ListDocToShow, "OperationNumber");
                ViewData["DocumentVariant"] = new SelectList(ListDocToShow, "DocumentTypeDescription");

            }
            return View(ListDocToShow);
        }
        [HttpPost]
        public ActionResult Index(int Index)
        {
            return View();
        }
        
        [HttpGet]
        public ActionResult Temp()
        {
            var sth = AppDomain.CurrentDomain;
            return View();
        }
        [HttpGet]
        public JsonResult EmpDetails()
        {
            List<DocumentAndTypeVM> ListDocToShow = new List<DocumentAndTypeVM>();
            using (var db = new DocumentDbContext())
            {
                foreach (var item in db.Documents.ToList())
                {
                    if (item.StatusId == 4 || item.StatusId == 7)
                    {
                        DocumentAndTypeVM temp = new DocumentAndTypeVM();
                        temp.Index = item.Index;
                        temp.DescriptionOp = db.Operations.Where(x => x.OperationId == item.OperationId).First().Description;
                        temp.OperationNumber = db.Operations.Where(x => x.OperationId == item.OperationId).First().OperationNumber;
                        temp.DocumentTypeDescription = db.DocumentTypes.Where(x => x.DocumentTypeId == item.DocumentTypeId).First().Description;
                        temp.LineName = db.Lines.Where(x => x.LineId == item.LineId).First().LineName;
                        temp.DocumentId = item.DocumentId;
                        ListDocToShow.Add(temp);
                    }
                }
            }
            return Json(ListDocToShow, JsonRequestBehavior.AllowGet);
        }
        
    }
}
