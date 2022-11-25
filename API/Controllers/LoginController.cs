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
        private IUserRepository _userRepository;

        private const String LoggedUser = "_User";

        //public delegate void UserLoggedEventHandler(object source, EventArgs args); 
        // public event UserLoggedEventHandler UserLogged;
        public event EventHandler<EventArgs> UserLogged;


        public LoginController(DataContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;

            UserLogged += _userRepository.OnUserLogged;

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
                    OnUserLogged();
                    return RedirectToAction("Index", "Home", new { user.Id });
                }

            }

            return RedirectToAction("Login");
        }

        protected virtual void OnUserLogged()
        {
            if (UserLogged != null)
                UserLogged(this, EventArgs.Empty);
        }
    }
}