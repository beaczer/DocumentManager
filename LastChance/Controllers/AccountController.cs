using LastChance.Models.LoginData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace LastChance.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            using (AccountDbContext db = new AccountDbContext())
            {
                return View(db.userAccount.ToList());
            }   
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserAccount account)
        {
            if(ModelState.IsValid)
            {
                using (AccountDbContext db = new AccountDbContext())
                {
                    db.userAccount.Add(account);
                    db.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.Message = account.FirstName + " " + account.LastName+ " " +"został(a) zarejstrowana pomyślnie";
            }
            return View();
        }
        //login
        public ActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Login(UserAccount user)
        {
            if (user.UserName!=null && user.Password!=null)
            {
                using (AccountDbContext db = new AccountDbContext())
                {
                    var usr = db.userAccount.Where(u => u.UserName == user.UserName && u.Password == user.Password).FirstOrDefault();
                    if (usr != null)
                    {
                        Session["UserId"] = usr.UserId.ToString();
                        Session["UserName"] = usr.UserName.ToString();
                        return RedirectToAction("LoggedIn");

                    }
                    else
                    {
                        ModelState.AddModelError("", "User Name lub Hasło jest nieprawidłowe");
                    }
                }
            }
            return View();
        }

        public ActionResult LoggedIn()
        {
            if(Session["UserId"]!=null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

    }
}