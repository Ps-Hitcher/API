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

namespace WebApplication2.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ICorrelationIDGenerator _correlationIdGenerator;
    private IUserRepository _userRepository;
    // public List<UserModel> _userList;
    DbSet<UserModel> _userList;

    private ITravelRepository _travelRepository;
    public DbSet<TravelModel> _travelList;
    private DbSet<ErrorViewModel> _errorList;
    // public List<TravelModel> _travelList;
    // public List<CarStruct> _carList;
    public HomeController(ILogger<HomeController> logger,
        IUserRepository userRepository, ITravelRepository travelRepository,
        ICorrelationIDGenerator correlationIdGenerator, DataContext context) //Using dependency injection for UserModel
    {
        _logger = logger; 
        _correlationIdGenerator = correlationIdGenerator;
        _userRepository = userRepository;
        _travelRepository = travelRepository;
        _userList = _userRepository.GetUserList();//debug
        _travelList = _travelRepository.GetTravelList();//debug
        _errorList = context.Errors;
    }


    [HttpGet]
    [Route("test/PrintMessage")]
    public string PrintMessage()
    {
        _logger.LogInformation("CorrelationId: {id}", _correlationIdGenerator.Get());
        return JsonConvert.SerializeObject(_userRepository.GetUserList());
    }
    
    public IActionResult Index()
    {
        throw new Exception("asdf");
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
    

    public IActionResult Trip(TravelModel trip)
    {
        trip = trip ?? new TravelModel();
        return View(trip);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        if(_errorList.Count() > 0)
        {
            return View(_errorList.OrderByDescending(ex => ex.DateAndTime).First());
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