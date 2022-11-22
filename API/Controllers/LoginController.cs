using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using WebApplication2.Data;
using WebApplication2.Models.User;

namespace WebApplication2.Controllers
{
    public class LoginController : Controller
    {
        private readonly DataContext _context;
        private const String LoggedUser = "_User";

        public LoginController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserModel model)
        {
            if (model != null)
            {
                var user = _context.Users.FirstOrDefault(s => s.Name == model.Name && s.Surname == model.Surname);
                if (user != null)
                {
                    HttpContext.Session.SetString(LoggedUser, user.Id.ToString());
                    return RedirectToAction("Index", "Home", new { user.Id });
                }

            }

            return RedirectToAction("Login");
        }
    }
}