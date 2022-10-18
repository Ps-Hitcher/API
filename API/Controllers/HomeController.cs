﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        return View(_userList);
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
        _userList.Remove(_userRepository.GetUser(id));
        
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
        
        return RedirectToAction("Users");
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
        if (!_userRepository.IsValidPhone(user.PhoneNumber))
        {
            TempData["UserModel"] = JsonConvert.SerializeObject(user);
            return RedirectToAction("Privacy", new { message = "Phone number is not valid" });
        }

        var obj = _userList.FirstOrDefault(x => x.Id == user.Id);

        if (obj != null)
        {
            _userList[_userList.FindIndex(x => x.Id == user.Id)] = user;
        }
        else
        {
            user.Id = Guid.NewGuid();
            _userList.Add(user);
        }
        
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