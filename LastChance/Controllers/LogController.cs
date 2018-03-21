using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LastChance.Controllers
{
    public class LogController : Controller
    {
        // GET: Log
        public ActionResult LogIn()
        {
            return View();
        }
        public ActionResult LogOut()
        {
            Session["UserId"] = null;
            Session["UserName"] = null;
            return View();
        }

    }
}