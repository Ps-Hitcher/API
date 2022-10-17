using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
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
    public List<UserModel> _userList;
    private ITravelRepository _travelRepository;
    public List<TravelModel> _travelList;

    public HomeController(ILogger<HomeController> logger, IUserRepository userRepository, ITravelRepository travelRepository) //Using dependency injection for UserModel
    {
        _logger = logger;
        _userRepository = userRepository;
        _travelRepository = travelRepository;
        _userList = _userRepository.GetUserList();//debug
        _travelList = _travelRepository.GetTravelList();//debug
    }

    public IActionResult Index()
    {
        return View(_travelList);
    }

    public IActionResult Privacy(UserModel user)
    {
        user = user ?? new UserModel();
        return View(user);
    }

    public IActionResult Users()
    {
        return View(_userList);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult AddUser(UserModel user)
    {
        user.Id = Guid.NewGuid();
        _userList.Add(user);

        try
        {
            //throw new Exception("Error here");  //For presentation
            string? jsonData = null;
            FileIO.WriteAllText(MockUserRepository._jsonPath, jsonData.SerializeJSON(_userList));
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            ErrorLogUtil.LogError(ex, "Adomas is responsible for this mess");
        }


        return RedirectToAction(nameof(Index));
    }

    public IActionResult AddTravel(TravelModel travel)
    {
        travel.TravelId = Guid.NewGuid();
        _travelList.Add(travel);

        try
        {
            //throw new Exception("Error here");  //For presentation
            string? jsonData = null;
            FileIO.WriteAllText(TravelRepository._jsonPath, jsonData.SerializeJSON(_travelList));
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            ErrorLogUtil.LogError(ex, "Adomas is responsible for this mess too");
        }


        return RedirectToAction(nameof(Index));
    }

}