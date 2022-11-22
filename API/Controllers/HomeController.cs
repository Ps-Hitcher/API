using System.Data;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.Models.User;
using WebApplication2.Models.Travel;
using FileIO = System.IO.File;
using WebApplication2.Models.Errors;

namespace WebApplication2.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ICorrelationIDGenerator _correlationIdGenerator;
    private IErrorRepository _errorRepository;
    private IUserRepository _userRepository;
    // public List<UserModel> _userList;
    DbSet<UserModel> _userList;

    private ITravelRepository _travelRepository;
    public DbSet<TravelModel> _travelList;
    // public List<TravelModel> _travelList;
    // public List<CarStruct> _carList;

    private const String LoggedUser = "_User";
    public HomeController(ILogger<HomeController> logger,
        IUserRepository userRepository, ITravelRepository travelRepository, IErrorRepository errorRepository,
        ICorrelationIDGenerator correlationIdGenerator, DataContext context) //Using dependency injection for UserModel
    {
        _logger = logger; 
        _correlationIdGenerator = correlationIdGenerator;
        _userRepository = userRepository;
        _travelRepository = travelRepository;
        _errorRepository = errorRepository;
        _userList = _userRepository.GetUserList();//debug
        _travelList = _travelRepository.GetTravelList();//debug
    }


    [HttpGet]
    [Route("test/PrintMessage")]
    public string PrintMessage()
    {
        var g = "hello";
        _logger.LogInformation("CorrelationId: {id}", _correlationIdGenerator.Get());
        return JsonConvert.SerializeObject(_errorRepository.GetErrorList());
    }
    
    public IActionResult Index(Guid id)
    {
        //HttpContext.Session.SetString(LoggedUser, id.ToString());
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
            // return View(new UserModel());
            // return View(_userList.FirstOrDefault(user => user.Id == Guid.Parse(HttpContext.Session.GetString(LoggedUser))));
            var StringId = HttpContext.Session.GetString(LoggedUser);
            Guid id = Guid.Parse(StringId);
            return View(_userRepository.GetUser(id));
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
    

    public IActionResult Trip(TravelModel trip)
    {
        trip = trip ?? new TravelModel();
        return View(trip);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        if(_errorRepository.GetErrorList().Count() > 0)
        {
            return View(_errorRepository.Get());
        }
        return RedirectToAction("Index");
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