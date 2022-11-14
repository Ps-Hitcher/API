using System.Diagnostics;
using System.Dynamic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApplication2.Models;
using WebApplication2.Models.User;
using WebApplication2.Models.Travel;
using WebApplication2.Utilities;
using FileIO = System.IO.File;

namespace WebApplication2.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    
    private IUserRepository _userRepository;
    public DbSet<UserModel> _userList;

    private ITravelRepository _travelRepository;
    public DbSet<TravelModel> _travelList;
    
    private IMetaRepository _metaRepository;
    public DbSet<MetaModel> _metaList;
    
    // public List<TravelModel> _travelList;
    // public List<CarStruct> _carList;
    public HomeController(ILogger<HomeController> logger, IUserRepository userRepository, ITravelRepository travelRepository, IMetaRepository metaRepository) //Using dependency injection for UserModel
    {
        _logger = logger;
        _userRepository = userRepository;
        _travelRepository = travelRepository;
        _metaRepository = metaRepository;
        _userList = _userRepository.GetUserList();//debug
        _travelList = _travelRepository.GetTravelList();//debug
        _metaList = _metaRepository.GetMetaList();//debug
    }

    public IActionResult Index()
    {
        return View(_travelList);
    }

    public IActionResult Privacy(string message)
    {
        ViewBag.Message = message;
        if (TempData["UserModel"] is not null)
        {
            UserModel user = JsonConvert.DeserializeObject<UserModel>((string)TempData["UserModel"]);
            return View(user);
        }
        else
        {
            return View(new UserModel());
        }
    }

    public IActionResult Edit(Guid id)
    {
        UserModel userM = _userRepository.GetUser(id);
        TempData["UserModel"] = JsonConvert.SerializeObject(userM);
        return RedirectToAction("Privacy");
    }

    public IActionResult Delete(Guid id)
    {
        _userRepository.DeleteUser(id);
        _userRepository.Save();
        
        return RedirectToAction("Users");
    }

    public IActionResult Users()
    {
        return View(_userList);
    }
    
    public IActionResult Datecher()
    {
        return View(_userList);
    }
    
    public IActionResult Trip(TravelModel trip, MetaModel meta)
    {
        ViewData["Trip"] = trip ?? new TravelModel();
        ViewData["Meta"] = meta ?? new MetaModel();
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult AddUser(UserModel user)
    {
        if (!_userRepository.IsValidPhone(user.PhoneNumber))
        {
            TempData["UserModel"] = JsonConvert.SerializeObject(user);
            return RedirectToAction("Privacy", new { message = "Phone number is not valid" });
        }

        var obj = _userList.AsNoTracking().FirstOrDefault(x => x.Id == user.Id);

        if (obj != null)
        {
            _userList.Update(user);
            _userRepository.Save();
        }
        else
        {
            user.Id = Guid.NewGuid();
            _userList.Add(user);
        }
        _userRepository.Save();

        return RedirectToAction(nameof(Index));
    }

    public IActionResult AddTravel(TravelModel travel)
    {
        travel.Id = Guid.NewGuid();
        _travelList.Add(travel);
        _userRepository.Save();

        return RedirectToAction(nameof(Index));
    }
    

    public IActionResult Login()
    {
        return View(Login);
    }

}