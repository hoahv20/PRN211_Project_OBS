using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PRN211_Project_OBS.Models;

namespace PRN211_Project_OBS.Controllers
{
    public class SignInController : Controller
    {
        DAO dao = new DAO();
        // GET: SignIn
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string email, string pass)
        {
            User x = dao.GetUserLogin(email, pass);
            if (x == null)
            {
                ViewBag.Mess = "Invalid email or password";
                return View();
            }
            Session["user"] = x;
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult SignUp(string email, string name, string pass, string repass)
        {
            if (dao.GetUserByEmail(email) != null)
            {
                ViewBag.Mess = "This Email has already been Registed!";
                return View();
            }
            if (pass.Length < 8)
            {
                ViewBag.Mess = "Password length must be equal or greater than 8 character.";
                return View();
            }
            if (!pass.Equals(repass))
            {
                ViewBag.Mess = "Password and Re-password must be similar.";
                return View();
            }
            dao.RegistUser(email, pass, name);
            Session["user"] = dao.GetUserByEmail(email);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogOut()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}