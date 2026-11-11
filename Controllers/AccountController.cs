using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using English_Vocabulary_Learning_Website.Models;

namespace English_Vocabulary_Learning_Website.Controllers
{
    public class AccountController : Controller
    {
        private VocabularyContext db = new VocabularyContext();

        // GET: Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserAccount user)
        {
            if (ModelState.IsValid)
            {
                var check = db.Users.FirstOrDefault(s => s.Username == user.Username);
                if (check == null)
                {
                    user.CreatedAt = DateTime.Now;
                    db.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.error = "Tên đăng nhập đã tồn tại!";
                    return View();
                }
            }
            return View();
        }

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            if (ModelState.IsValid)
            {
                var data = db.Users.FirstOrDefault(s => s.Username.Equals(username) && s.Password.Equals(password));
                if (data != null)
                {
                    // Lưu thông tin người dùng vào Session
                    Session["UserId"] = data.Id;
                    Session["Username"] = data.Username;
                    return RedirectToAction("Index", "Learn");
                }
                else
                {
                    ViewBag.error = "Tên đăng nhập hoặc mật khẩu không đúng!";
                    return View();
                }
            }
            return View();
        }

        // Logout
        public ActionResult Logout()
        {
            Session.Clear(); // Xóa Session
            return RedirectToAction("Login");
        }
    }
}