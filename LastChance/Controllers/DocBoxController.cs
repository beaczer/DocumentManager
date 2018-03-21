using DocumentFormat.OpenXml.Wordprocessing;
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
    public class DocBoxController : Controller
    {
        private static Microsoft.Office.Interop.Excel.Workbook MyBook = null;
        private static Microsoft.Office.Interop.Excel.Application MyApp = null;
        private static Microsoft.Office.Interop.Excel.Worksheet MyWorkSheet = null;

        // GET: DocBox
        public ActionResult Index()
        {
            List<UserBoxVM> listDocToAccep = new List<UserBoxVM>();
            List<UserBoxVM> listAccepted = new List<UserBoxVM>();
            List<UserBoxVM> listRejected = new List<UserBoxVM>();
            List<UserBoxVM> AllKindOfDocuments = new List<UserBoxVM>();
            UserAccount tempAcceptUser;
            User acceptUser;
            AcceptedUser acceUser;
            List<Models.DocumentData.Document> listaTemp = new List<Models.DocumentData.Document>();
            var user = (string)Session["UserName"];
            
            using (var db = new DocumentDbContext())
            {
                using (var db1 = new AccountDbContext())
                {
                    tempAcceptUser = db1.userAccount.Where(x => x.UserName == user).First();
                    acceptUser = db.Users.Where(x => x.LastName == tempAcceptUser.LastName).First();
                    listAccepted = GetList(2, acceptUser);
                    listRejected = GetList(7, acceptUser);
                    if (acceptUser.AcceptUser != null)
                    {
                        acceUser = db.UserAccept.Where(x => x.User.UserId == acceptUser.UserId).First();
                        foreach (var item in db.Documents.Where(x => x.DateOfAcceptance == null &&
                        x.Status.StatusId == 1 && x.AccepedUser.AcceptedUserId == acceUser.AcceptedUserId).ToList())
                        {
                            UserBoxVM temp = new UserBoxVM();
                            temp.DocumentId = item.DocumentId;
                            temp.DateOfAccept = item.DateOfAcceptance;
                            temp.FileToCheck = item.DocumentFile;
                            temp.Status = item.Status.Description;
                            temp.Index = item.Index;
                            listDocToAccep.Add(temp);
                        }
                    }
                }
            }
            var result= listDocToAccep.Concat(listAccepted);
            var res = result.Concat(listRejected);
            AllKindOfDocuments = res.ToList();
            return View(AllKindOfDocuments);
        }
        private List<UserBoxVM> GetList(int status, User interested)
        {
            var user = (string)Session["UserName"];
            UserAccount tempAcceptUser;
            User acceptUser;
            AuthorUser authorUser;
            User deleteUser;
            List<UserBoxVM> tempList = new List<UserBoxVM>();
            using (var db = new DocumentDbContext())
            {
                using (var db1 = new AccountDbContext())
                {
                    tempAcceptUser = db1.userAccount.Where(x => x.UserName == user).First();
                    acceptUser = db.Users.Where(x => x.LastName == tempAcceptUser.LastName).First();
                    deleteUser = db.Users.Where(x => x.LastName == tempAcceptUser.LastName).First();
                }
                if (status==2)
                {
                    authorUser = db.UserAuthor.Where(x => x.User.UserId == acceptUser.UserId).First();
                    foreach (var item in db.Documents.Where(x=>x.Status.StatusId == 2 && 
                    x.AuthorUserId == authorUser.AuthorUserId).ToList())
                    {
                        UserBoxVM temp = new UserBoxVM();
                        temp.DocumentId = item.DocumentId;
                        temp.DateOfAccept = item.DateOfAcceptance;
                        temp.FileToCheck = item.DocumentFile;
                        temp.Status = item.Status.Description;
                        temp.Index = item.Index;
                        tempList.Add(temp);
                    }
                }
                if(status==7)
                {
                    foreach (var item in db.Documents.Where(x=>x.StatusId==7 && x.DeleteUserId==deleteUser.UserId).ToList())
                    {
                        UserBoxVM temp = new UserBoxVM();
                        temp.DocumentId = item.DocumentId;
                        temp.DateOfAccept = item.DateOfAcceptance;
                        temp.FileToCheck = item.DocumentFile;
                        temp.Status = item.Status.Description;
                        temp.Index = item.Index;
                        tempList.Add(temp);
                    }
                }

            }
            return tempList;
        }
        public ActionResult PutToArchive(int docId)
        {
            //5 archiwum
            using (var db = new DocumentDbContext())
            {
                db.Documents.Where(x => x.DocumentId == docId).First().Status = db.Status.Where(c => c.StatusId == 5).First();
                db.Documents.Where(x => x.DocumentId == docId).First().DateOfDelete = DateTime.Today;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult CancelPutToArchive(int docId)
        {
            //Powrot do status id=4
            using (var db = new DocumentDbContext())
            {
                db.Documents.Where(x => x.DocumentId == docId).First().Status = db.Status.Where(c => c.StatusId == 4).First();
                db.Documents.Where(x => x.DocumentId == docId).First().DateOfDelete = null;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult MakeActive(int docId)
        {
            //4 status wydana na linie
            using (var db = new DocumentDbContext())
            {
                var DocExcel = db.Documents.Where(x => x.DocumentId == docId).First().DocumentFileExcel;
                string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string pathDownload = Path.Combine(pathUser, "Downloads");
                string title = "temp" + DateTime.Now;
                title = title.Replace("-", "");
                title = title.Replace(":", "");
                string title2 = "temp" + ".pdf";
                string EndPathPDF = Path.Combine(pathDownload, title2);
                string EndPath = Path.Combine(pathDownload, title);

                int VersionCurrent = db.Documents.Where(x => x.DocumentId == docId).First().Version;
                int VersionPrevious = VersionCurrent-1;
                if (VersionCurrent>1)
                {
                    string index = db.Documents.Where(x => x.DocumentId == docId).First().Index;

                    Models.DocumentData.Document DocumentToArchive = db.Documents.Where(x => x.Index == index).Where(x => x.Version == VersionPrevious).First();
                    DocumentToArchive.Status = db.Status.Where(x => x.StatusId == 5).First();
                    db.SaveChanges();
                }


                //stworzenie tablicy wielkosci 
                System.IO.File.WriteAllBytes(EndPath, DocExcel);
                MyApp = new Microsoft.Office.Interop.Excel.Application();
                MyApp.Visible = false;
                MyBook = MyApp.Workbooks.Open(EndPath);
                MyWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)MyBook.Sheets[1];
                MyWorkSheet.Cells[3, 13] = DateTime.Now.ToShortDateString();
                MyApp.Save();
                MyBook.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, EndPathPDF);
                MyBook.Close();
                byte[] f1 = new byte[15728640];
                f1 = System.IO.File.ReadAllBytes(EndPath);
                byte[] fPDF = new byte[15728640];
                fPDF = System.IO.File.ReadAllBytes(EndPathPDF);

//zamiana indeksu z 4 na 5 w poprzedniej wersji

                db.Documents.Where(x => x.DocumentId == docId).First().DocumentFile = fPDF;
                db.Documents.Where(x => x.DocumentId == docId).First().DocumentFileExcel = f1;
                db.Documents.Where(x => x.DocumentId == docId).First().Status = db.Status.Where(c => c.StatusId == 4).First();
                
                db.Documents.Where(x => x.DocumentId == docId).First().DateOfPublic = DateTime.Today;
                db.SaveChanges();
                if (System.IO.File.Exists(EndPathPDF)) System.IO.File.Delete(EndPathPDF);
                if (System.IO.File.Exists(EndPath)) System.IO.File.Delete(EndPath);
            }
            return RedirectToAction("Index");
        }
        public ActionResult CancelDocument(int docId)
        {
            //9 wycofana z obiegu
            using (var db = new DocumentDbContext())
            {
                db.Documents.Where(x => x.DocumentId == docId).First().Status = db.Status.Where(c => c.StatusId == 9).First();
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        //private byte[] GetExcelFile()
        //{
            
        //}
        public ActionResult SayYes(int docId)
        {
            //2 status zatwierdzona w bazie danych
            using (var db = new DocumentDbContext())
            {
                var DocExcel = db.Documents.Where(x => x.DocumentId == docId).First().DocumentFileExcel;
                string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string pathDownload = Path.Combine(pathUser, "Downloads");
                string title = "temp"  + DateTime.Now;
                title = title.Replace("-", "");
                title = title.Replace(":", "");
                string title2 = "temp"+ ".pdf";
                string EndPathPDF = Path.Combine(pathDownload, title2);
                string EndPath = Path.Combine(pathDownload, title);
                
                //stworzenie tablicy wielkosci 
                System.IO.File.WriteAllBytes(EndPath, DocExcel);
                MyApp = new Microsoft.Office.Interop.Excel.Application();
                MyApp.Visible = false;
                MyBook = MyApp.Workbooks.Open(EndPath);
                MyWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)MyBook.Sheets[1];
                var acceptUser = db.Documents.Where(x => x.DocumentId == docId).First().AcceptedUserId;
                string user = db.Users.Where(x => x.AcceptUser.AcceptedUserId == acceptUser).First().FirstName + " "+
                              db.Users.Where(x => x.AcceptUser.AcceptedUserId == acceptUser).First().LastName;
                MyWorkSheet.Cells[5, 13] = user;
                MyApp.Save();
                MyBook.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, EndPathPDF);
                MyBook.Close();
                byte[] f1 = new byte[15728640];
                f1 = System.IO.File.ReadAllBytes(EndPath);
                byte[] fPDF = new byte[15728640];
                fPDF = System.IO.File.ReadAllBytes(EndPathPDF);

                db.Documents.Where(x => x.DocumentId == docId).First().DocumentFile = fPDF;
                db.Documents.Where(x => x.DocumentId == docId).First().DocumentFileExcel = f1;
                db.Documents.Where(x => x.DocumentId == docId).First().Status=db.Status.Where(c=>c.StatusId==2).First();
                db.Documents.Where(x => x.DocumentId == docId).First().DateOfAcceptance = DateTime.Today;
                db.SaveChanges();
            }
                return RedirectToAction("Index");
        }
        
        public ActionResult SayNo(int docId)
        {
            //3 status odrzucona w bazie danych
            using (var db = new DocumentDbContext())
            {
                db.Documents.Where(x => x.DocumentId == docId).First().Status = db.Status.Where(c => c.StatusId == 3).First();
                db.Documents.Where(x => x.DocumentId == docId).First().DateOfAcceptance = DateTime.Today;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult GoToArchive(int docId)
        {
            return RedirectToAction("Index");
        }
        public ActionResult AddComment(int docId)
        {
            using (var db = new DocumentDbContext())
            {
                Models.DocumentData.Document document = db.Documents.Where(x => x.DocumentId == docId).First();

                return View(document);
            }
        }

        public ActionResult AddComments(string doc, int id)
        {
            using (var db = new DocumentDbContext())
            {
                db.Documents.Where(x => x.DocumentId == id).First().Comments = doc;
                db.SaveChanges();
            }
                return RedirectToAction("Index");
        }
        private List<UserBoxVM> GetListOfDoccumentToAccept()
        {
            List<UserBoxVM> listDocToAccep = new List<UserBoxVM>();
            UserAccount tempAcceptUser;
            User acceptUser;
            List<Models.DocumentData.Document> listaTemp = new List<Models.DocumentData.Document>();
            var user = (string)Session["UserName"];
           
            using (var db = new DocumentDbContext())
            {
                using (var db1 = new AccountDbContext())
                {
                    tempAcceptUser = db1.userAccount.Where(x => x.UserName == user).First();
                    acceptUser = db.Users.Where(x => x.LastName == tempAcceptUser.LastName).First();

                    foreach (var item in db.Documents.Where(x => x.DateOfAcceptance == null && x.Status.StatusId == 1).ToList())
                    {
                        UserBoxVM temp = new UserBoxVM();
                        temp.DocumentId = item.DocumentId;
                        temp.DateOfAccept = item.DateOfAcceptance;
                        temp.FileToCheck = item.DocumentFile;
                        temp.Status= item.Status.Description;
                        temp.Index = item.Index;
                        listDocToAccep.Add(temp);
                    }
                }
            }
            return listDocToAccep;
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
                temp = new byte[db.Documents.Where(x => x.DocumentId == docId).First().DocumentFile.Length];
                temp = db.Documents.Where(x => x.DocumentId == docId).First().DocumentFile;
                title = db.Documents.Where(x => x.DocumentId == docId).First().Title + ".pdf";
                EndPath = Path.Combine(pathDownload, title);
                System.IO.File.WriteAllBytes(EndPath, temp);

            }
            System.Diagnostics.Process.Start(EndPath);
            return RedirectToAction("Index");
        }
    }


}