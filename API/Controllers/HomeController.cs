using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using WebApplication2.Models.User;

namespace WebApplication2.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IUserRepository _userRepository;
    public List<UserModel> _userList;
    public HomeController(ILogger<HomeController> logger, IUserRepository userRepository) //Using dependency injection for UserModel
    {
        _logger = logger;
        _userRepository = userRepository;
        _userList = _userRepository.GetUserList();//debug
    }

    public IActionResult Index()
    {
        return View(_userList);
    }

    public IActionResult Privacy(UserModel user)
    {
        user = user ?? new UserModel();
        return View(user);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult AddUser(UserModel user)
    {
        if (_userRepository.IsValidPhone(user.PhoneNumber))
        {
            user.Id = Guid.NewGuid();
            _userList.Add(user);
            return RedirectToAction(nameof(Index));
        }
        else{
            return RedirectToAction(nameof(Privacy));
        }
    }

    public IActionResult Users()
    {
        return View(Users());
    }
} 