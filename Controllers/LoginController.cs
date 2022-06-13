using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVCBankProjectUsingEFCore.Models;
using Microsoft.AspNetCore.Http;

namespace MVCBankProjectUsingEFCore.Controllers
{
    public class LoginController : Controller
    {
        private readonly SharpBankContext _context;

        public LoginController(SharpBankContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Login(Authenticate user)
        {
            user = (from i in _context.Authenticates
                    where i.UserName == user.UserName && i.Password == user.Password
                    select i).FirstOrDefault();
            if(user != null)
            {
                HttpContext.Session.SetString("mailID", user.UserName);
                HttpContext.Session.SetString("fName", user.FirstName);
                HttpContext.Session.SetString("lName", user.LastName);
                HttpContext.Session.SetString("popped", "no");
                return RedirectToAction("Index", "Home");
            }
            return View("Login", "Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("mailID");
            HttpContext.Session.Remove("fName");
            HttpContext.Session.Remove("lName");
            return View("Login");
        }
    }

}
