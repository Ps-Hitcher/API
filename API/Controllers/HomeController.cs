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
using WebApplication2.Utilities;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WebApplication2.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    
    private readonly IUserRepository _userRepository;
    private DbSet<UserModel> _userList;

    private readonly ITravelRepository _travelRepository;
    private DbSet<TravelModel> _travelList;
    
    private readonly IMetaRepository _metaRepository;
    private DbSet<MetaModel> _metaList;
    
    private readonly ICorrelationIDGenerator _correlationIdGenerator;
    private IErrorRepository _errorRepository;
    
    private const String LoggedUser = "_User";
    public HomeController(ILogger<HomeController> logger,
        IUserRepository userRepository, ITravelRepository travelRepository, 
        IMetaRepository metaRepository, IErrorRepository errorRepository, 
        ICorrelationIDGenerator correlationIdGenerator, DataContext context)
        //Using dependency injection for UserModel
    {
        _logger = logger; 
        _correlationIdGenerator = correlationIdGenerator;
        _userRepository = userRepository;
        _travelRepository = travelRepository;
        _metaRepository = metaRepository;
        _errorRepository = errorRepository;
        _userList = _userRepository.GetUserList();//debug
        _travelList = _travelRepository.GetTravelList();//debug
        _metaList = _metaRepository.GetMetaList();//debug
    }

    
    [HttpGet]
    [Route("test/PrintMessage")]
    public string PrintMessage()
    {
        var g = "hello";
        _logger.LogInformation("CorrelationId: {id}", _correlationIdGenerator.Get());
        return JsonConvert.SerializeObject(_errorRepository.GetErrorList());
    }
    
    [HttpGet]
    public IActionResult Index()
    {
        var data = new SearchTravel();
        data.TravelModel!.TravelResults = _travelList.ToList();
        return View(data);
    }
    // [HttpGet]
    // public IActionResult Index()
    // {
    //     return View(null);
    // }
    
    [HttpPost]
    public IActionResult Index(SearchTravel t)
    {
        if (ModelState.IsValid)
        {
            var data = new SearchTravel();
            data.TravelModel!.TravelResults = _travelList.ToList();
            return View(null);
        }
        Console.WriteLine("Origin Lat - " + t.OriginLat);
        Console.WriteLine("Origin Lng - " + t.OriginLng);
        Console.WriteLine("Destination Lat - " + t.DestinationLat);
        Console.WriteLine("Destination Lng - " + t.DestinationLng);
        // t = new TravelModel();
        IEnumerable<MetaModel> queriedMetaContext = _metaList.ToList();
        IEnumerable<TravelModel> queriedTripList = Enumerable.Empty<TravelModel>();
        IEnumerable<UserModel> queriedUserList = Enumerable.Empty<UserModel>();
        IEnumerable<MetaModel> queriedMetaList = queriedMetaContext;
        foreach (var travel in _travelList)
        {
            // var queriedTripMetaContext = _metaList.Where(e => e.TravelId == travel.Id).ToList();
            var queriedTripMetaList =
                from meta in queriedMetaContext where travel.Id == meta.TravelId select meta;
            if ((t.OriginLat != null) && (t.DestinationLat != null))
            {
                queriedMetaList = queriedTripMetaList.Where(e =>
                    (Math.Abs(e.OriginLat - (double)t.OriginLat) <= 0.12 &&
                        Math.Abs(e.OriginLng - (double)t.OriginLng) <= 0.2) &&
                    (Math.Abs(e.DestinationLat - (double)t.DestinationLat) <= 0.12 &&
                        Math.Abs(e.DestinationLng - (double)t.DestinationLng) <= 0.2));
                // queriedMetaList =
                //     from meta in queriedTripMetaList
                //     where (
                //         (Math.Abs(meta.OriginLat - (double)t.OriginLat) <= 0.12 &&
                //          Math.Abs(meta.OriginLng - (double)t.OriginLng) <= 0.2) &&
                //         (Math.Abs(meta.DestinationLat - (double)t.DestinationLat) <= 0.12 &&
                //          Math.Abs(meta.DestinationLng - (double)t.DestinationLng) <= 0.2))
                //     select meta;
            }
            else if (t.OriginLat != null)
            {
                // queriedMetaContext = queriedTripMetaList.Where(e =>
                //     (Math.Abs(e.OriginLat - (double)t.OriginLat) <= 0.12 &&
                //          Math.Abs(e.OriginLng - (double)t.OriginLng) <= 0.2));
                queriedMetaList = 
                    from meta in queriedTripMetaList
                    where 
                        (Math.Abs(meta.OriginLat - (double)t.OriginLat) <= 0.12 && 
                         Math.Abs(meta.OriginLng - (double)t.OriginLng) <= 0.2)
                        select meta;
            }
            else if (t.DestinationLat != null)
            {
                queriedMetaList = queriedTripMetaList.Where(e =>
                    (Math.Abs(e.DestinationLat - (double)t.DestinationLat) <= 0.12 &&
                        Math.Abs(e.DestinationLng - (double)t.DestinationLng) <= 0.2));
            }
            // var diffLat = lat2 - lat1; 
            // var diffLng = lon2 - lon1;
            //
            // return (diffLat <= 0.12) && (diffLng <= 0.2);
            else
            {
                var data = new SearchTravel();
                data.TravelModel!.TravelResults = _travelList.ToList();
                return View(null);
            }
            if (queriedMetaList.Any())
            {
                queriedTripList = queriedTripList.Append(travel);
            }
        }
        t.TravelModel.TravelResults = queriedTripList.ToList();
        foreach (var trip in queriedTripList)
        {
            foreach (var user in _userList)
            {
                if (user.Id == trip.DriverId)
                {
                    queriedUserList = queriedUserList.Append(user);
                }
            }
        }
        var results = new SearchResults
        {
            SearchTravel = t,
            UserModel = queriedUserList.ToList()
        };

        return RedirectToAction("Datecher", "Home", results);
    }
    // public IActionResult Index(double lat1, double lng1, double lat2, double lng2)
    // {
    //     if (_travelList.Count() > 0)
    //     {
    //         _travelList = _travelRepository.GetTravelList().Where(e => (TravelFilter.CloseCoords(lat1, lng1, 54.6989925, 25.2576996) && TravelFilter.CloseCoords(lat2, lng2, 25.2627452, 54.6719751)));
    //     }
    //     
    //     
    //     
    //     return View(_travelList);
    // }
    //
    // public void GetTripInfo()

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

    [HttpGet]
    public IActionResult Datecher(SearchResults info)
    {
        
        
        
        
        return View(info);
        
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
        var stopStr = new List<string>(input.Stopovers.Split(";"));
        stopStr.Remove(input.Destination);
        
        TravelModel travel = new TravelModel
        {
            Id = travelId,
            Origin = input.Origin,
            Destination = input.Destination,
            Stopovers = stopStr,
            Time = input.Time,
            DriverId = input.DriverID,
            FreeSeats = input.FreeSeats,
            Description = input.Description
        };
        _travelList.Add(travel);

        
        List<string> stopoverList = new List<string>(input.Stopovers.Split(";"));
        List<double> bearingList = new List<double>(Array.ConvertAll(input.Bearings.Split(","), Double.Parse));
        List<double> distanceList = new List<double>(Array.ConvertAll(input.Distance.Split(","), Double.Parse));
        List<double> latList = new List<double>(Array.ConvertAll(input.Lat.Split(","), Double.Parse));
        List<double> lngList = new List<double>(Array.ConvertAll(input.Lng.Split(","), Double.Parse));
        for (var i = 0; i < stopoverList.Count; i++)
        {
            MetaModel meta = new MetaModel();
            meta.TravelId = travelId;
            if (i == 0)
            {
                meta.Origin = input.Origin;
            }
            else
            {
                meta.Origin = stopoverList[i - 1];
            }
            meta.Destination = stopoverList[i];
            meta.Bearing = Convert.ToDouble(bearingList[i]);
            meta.Distance = Convert.ToDouble(distanceList[i]);
            meta.OriginLat = Convert.ToDouble(latList[i]);
            meta.OriginLng = Convert.ToDouble(lngList[i]);
            meta.DestinationLat = Convert.ToDouble(latList[i + 1]);
            meta.DestinationLng = Convert.ToDouble(lngList[i + 1]);
            _metaList.Add(meta);
        }

        _travelRepository.Save();
        _metaRepository.Save();

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Login()
    {
        return View(Login);
    }

}
