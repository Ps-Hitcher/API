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
    }

    public IActionResult Index()
    {
        _userList = _userRepository.GetUserList();
        return View(_userList);
    }
    
    // public string Index()
    // {
    //     return _userRepository.GetUser(2).Name;//Testing
    // }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}