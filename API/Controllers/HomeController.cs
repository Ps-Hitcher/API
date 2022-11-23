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
    
    private readonly IUserRepository _userRepository;
    private readonly DbSet<UserModel> _userList;

    private readonly ITravelRepository _travelRepository;
    private readonly DbSet<TravelModel> _travelList;
    
    private readonly IMetaRepository _metaRepository;
    private readonly DbSet<MetaModel> _metaList;
    
    private readonly ICoordsRepository _coordsRepository;
    private readonly DbSet<CoordsModel> _coordsList;
    
    private readonly ICorrelationIDGenerator _correlationIdGenerator;
    private IErrorRepository _errorRepository;
    
    private const String  LoggedUser = "_User";
    public HomeController(ILogger<HomeController> logger,
        IUserRepository userRepository, ITravelRepository travelRepository, 
        IMetaRepository metaRepository, ICoordsRepository coordsRepository, 
        IErrorRepository errorRepository, ICorrelationIDGenerator correlationIdGenerator, DataContext context)
        //Using dependency injection for UserModel
    {
        _logger = logger; 
        _correlationIdGenerator = correlationIdGenerator;
        _userRepository = userRepository;
        _travelRepository = travelRepository;
        _metaRepository = metaRepository;
        _coordsRepository = coordsRepository;
        _errorRepository = errorRepository;
        _userList = _userRepository.GetUserList();//debug
        _travelList = _travelRepository.GetTravelList();//debug
        _metaList = _metaRepository.GetMetaList();//debug
        _coordsList = _coordsRepository.GetCoordsList();//debug
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
    
    public IActionResult Trip(FormInput? input)
    {
        ViewData["Input"] = input ?? new FormInput();
        return View();
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

    public IActionResult AddTravel(FormInput input)
    {
        var travelId = Guid.NewGuid();
        
        TravelModel travel = new TravelModel
        {
            Id = travelId,
            Origin = input.Origin,
            Destination = input.Destination,
            Stopovers = new List<string>(input.Stopovers.Split(";")),
            Time = input.Time,
            DriverID = input.DriverID,
            FreeSeats = input.FreeSeats,
            Description = input.Description
        };
        _travelList.Add(travel);

        
        List<string> stopoverList = new List<string>(input.Stopovers.Split(";"));
        List<double> bearingList = new List<double>(Array.ConvertAll(input.Bearings.Split(","), Double.Parse));
        List<double> distanceList = new List<double>(Array.ConvertAll(input.Distance.Split(","), Double.Parse));
        for (var i = 0; i < stopoverList.Count; i++)
        {
            MetaModel meta = new MetaModel();
            meta.TravelId = travelId;
            meta.MetaDestination = stopoverList[i];
            meta.Bearing = Convert.ToDouble(bearingList[i]);
            meta.Distance = Convert.ToDouble(distanceList[i]);
            _metaList.Add(meta);
        }
        

        _travelRepository.Save();
        _metaRepository.Save();
        _coordsRepository.Save();

        return RedirectToAction(nameof(Index));
    }
    

    public IActionResult Login()
    {
        return View(Login);
    }

}
