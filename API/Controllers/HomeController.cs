﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using WebApplication2.Models.User;
using WebApplication2.Utilities;
using FileIO = System.IO.File;

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
        //_userList = _userRepository.GetUserList();
        return View(_userList);
    }
    // public string Index()
    // {
    //     return _userRepository.GetUser(2).Name;//Testing
    // }

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
        catch(Exception ex)
        {
            Debug.WriteLine(ex.Message);
            ErrorLogUtil.LogError(ex, "Adomas is responsible for this mess");
        }


        return RedirectToAction(nameof(Index));
    }

}