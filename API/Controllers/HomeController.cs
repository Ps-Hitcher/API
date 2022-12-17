﻿using System.Data;
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
        data.TravelResults = _travelList.ToList();
        return View(data);
    }
    
    [HttpPost]
    public IActionResult Index(SearchTravel t)
    {
        if (!ModelState.IsValid)
        {
            var data = new SearchTravel();
            data.TravelResults = _travelList.ToList();
            return View(data);
        }
        Console.WriteLine("Origin Lat - " + t.OriginLat);
        Console.WriteLine("Origin Lng - " + t.OriginLng);
        Console.WriteLine("Destination Lat - " + t.DestinationLat);
        Console.WriteLine("Destination Lng - " + t.DestinationLng);
        SearchInfo searchInfo = new SearchInfo()
        {
            Origin = t.Origin,
            OriginLat = t.OriginLat,
            OriginLng = t.OriginLng,
            Destination = t.Destination,
            DestinationLat = t.DestinationLat,
            DestinationLng = t.DestinationLng,
            Bearings = t.Bearings
        };
        // t = new TravelModel();
        IEnumerable<MetaModel> queriedMetaContext = _metaList.ToList();
        IEnumerable<TravelModel> queriedTripList = Enumerable.Empty<TravelModel>();
        IEnumerable<UserModel> queriedUserList = Enumerable.Empty<UserModel>();
        var LatLng = "\"";
        foreach (var travel in _travelList)
        {
            IEnumerable<MetaModel> queriedMetaList = 
                from meta in queriedMetaContext where travel.Id == meta.TravelId select meta;
            if (t is { OriginLat: { }, DestinationLat: { } })
            {
                if (TravelFilter.RelevantRideFull(searchInfo, queriedMetaList, travel.Origin, travel.Destination))
                {
                    queriedTripList = queriedTripList.Append(travel);
                    LatLng += TravelFilter.CoordConstuctor(queriedMetaList, travel.Origin, travel.Destination) + ";";
                }
            }
            else if (t.OriginLat != null)
            {
                if (TravelFilter.RelevantRideOrigin(searchInfo, queriedMetaList))
                {
                    queriedTripList = queriedTripList.Append(travel);
                    LatLng += TravelFilter.CoordConstuctor(queriedMetaList, travel.Origin, travel.Destination);
                }
            }
            else if (t.DestinationLat != null)
            {
                if (TravelFilter.RelevantRideDestination(searchInfo, queriedMetaList))
                {
                    queriedTripList = queriedTripList.Append(travel);
                    LatLng += TravelFilter.CoordConstuctor(queriedMetaList, travel.Origin, travel.Destination);
                }
            }
            else
            {
                var data = new SearchTravel();
                data.TravelResults = _travelList.ToList();
                return View(null);
            }
        }
        t.TravelResults = queriedTripList.ToList();
        queriedUserList = (from trip in queriedTripList from user in _userList where user.Id == trip.DriverId select user).Aggregate(queriedUserList, (current, user) => current.Append(user));
        
        var results = new SearchResults
        {
            Origin = t.Origin,
            OriginLat = t.OriginLat,
            OriginLng = t.OriginLng,
            Destination = t.Destination,
            DestinationLat = t.DestinationLat,
            DestinationLng = t.DestinationLng,
            Bearings = t.Bearings,
            TravelUser = new TravelUser(LatLng + "\"", queriedTripList.ToList(), queriedUserList.ToList())
        };
        TempData.Put("results", results);
        return RedirectToAction("Datecher", "Home");
    }

    public IActionResult Privacy(string message)
    {
        ViewBag.Message = message;
        if (TempData["UserModel"] is not null)
        {
            UserModel user = JsonConvert.DeserializeObject<UserModel>((string)TempData["UserModel"]);
            return View("Privacy", user);
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
        return View("Users", _userList);
    }
    
    public IActionResult MyTrip()
    {
        return View("MyTrips", _userList);
    }

    [HttpGet]
    public IActionResult Datecher()
    {
        SearchResults info = TempData.Get<SearchResults>("results");
        
        return View("Datecher", info);
    }
    
    public IActionResult Calculator()
    { 
        var StringId = HttpContext.Session.GetString(LoggedUser);
        Guid id = Guid.Parse(StringId);
        var name = _userRepository.GetUser(id).Name;
        var zodiac = _userRepository.GetHoroName_(_userRepository.GetUser(id).YearOfBirth);
        ViewBag.Message = name + " ★ " + zodiac;
        return View("Calculator" ,_userList);
    }
    
    public IActionResult Trip(FormInput? input)
    {
        ViewData["Input"] = input ?? new FormInput();
        return View("Trip");
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
        var StringId = HttpContext.Session.GetString(LoggedUser);
        Guid id = Guid.Parse(StringId);
        var user = _userRepository.GetUser(id);
        
        TravelModel travel = new TravelModel
        {
            Id = travelId,
            Origin = input.Origin,
            Destination = input.Destination,
            Stopovers = stopStr,
            Time = input.Time,
            DriverId = user.Id,
            DriverName = user.Name,
            DriverSurname = user.Surname,
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

    // public IActionResult Login()
    // {
    //     return View(Login);
    // }

}
