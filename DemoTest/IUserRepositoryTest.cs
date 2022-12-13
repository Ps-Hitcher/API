using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2;
using WebApplication2.Models.User;
using System;
using System.Reflection.PortableExecutable;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework.Internal;
using WebApplication2.Controllers;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.Models.Errors;
using WebApplication2.Models.Travel;


public class IUserRepositoryTest
{
    private UserModel _user1;
    private UserModel _user2;


    [SetUp]
    public void Setup()
    {
        _user1 = new UserModel() { Name = "Kamile", Surname = "Samusiovaite", YearOfBirth = "2002-02-12", PhoneNumber = "860569664" };
        _user2 = new UserModel() { Name = "Domas", Surname = "Nemanius", YearOfBirth = "2023-09-12" };
    }

    [Test]
    public void GetUserAge_ShouldRetunOkResponse_WhenReturnsTheRightAnswer()
    {
        Assert.AreEqual(IUserRepository.GetUserAge((_user1.YearOfBirth)), 20);
        Assert.AreEqual(IUserRepository.GetUserAge((_user2.YearOfBirth)), -1);
        Assert.AreEqual(IUserRepository.GetUserAge("2002-12-20"), 19);

    }
    
    [Test]
    public void GetHoroName_ShouldRetunOkResponse_WhenReturnsTheRightAnswer()
    {
        Assert.AreEqual(IUserRepository.GetHoroName("2005-01-19"), "Capricorn");
        Assert.AreEqual(IUserRepository.GetHoroName("2005-01-20"), "Aquarius");
        Assert.AreEqual(IUserRepository.GetHoroName("2005-02-18"), "Aquarius");
        Assert.AreEqual(IUserRepository.GetHoroName("2005-02-19"), "Pisces");
        Assert.AreEqual(IUserRepository.GetHoroName("2005-03-19"), "Pisces");
        Assert.AreEqual(IUserRepository.GetHoroName("2005-03-21"), "Aries");
        Assert.AreEqual(IUserRepository.GetHoroName("2005-04-19"), "Aries");
        Assert.AreEqual(IUserRepository.GetHoroName("2005-04-20"), "Taurus");
        Assert.AreEqual(IUserRepository.GetHoroName("2005-05-20"), "Taurus");
        Assert.AreEqual(IUserRepository.GetHoroName("2005-05-21"), "Gemini");
        Assert.AreEqual(IUserRepository.GetHoroName("2005-06-20"), "Gemini");
        Assert.AreEqual(IUserRepository.GetHoroName("2005-06-21"), "Cancer");
        Assert.AreEqual(IUserRepository.GetHoroName("2005-07-22"), "Cancer");
        Assert.AreEqual(IUserRepository.GetHoroName("2005-07-23"), "Leo");
        Assert.AreEqual(IUserRepository.GetHoroName("2005-08-22"), "Leo");
        Assert.AreEqual(IUserRepository.GetHoroName("2005-09-22"), "Virgo");
        Assert.AreEqual(IUserRepository.GetHoroName("2005-08-23"), "Virgo");
        Assert.AreEqual(IUserRepository.GetHoroName("2005-10-22"), "Libra");
        Assert.AreEqual(IUserRepository.GetHoroName("2005-09-23"), "Libra");
        Assert.AreEqual(IUserRepository.GetHoroName("2005-10-23"), "Scorpio");
        Assert.AreEqual(IUserRepository.GetHoroName("2005-11-21"), "Scorpio");
        Assert.AreEqual(IUserRepository.GetHoroName("2005-11-22"), "Sagittarius");
        Assert.AreEqual(IUserRepository.GetHoroName("2005-12-21"), "Sagittarius");
        Assert.AreEqual(IUserRepository.GetHoroName("2005-12-22"), "Capricorn");
        
    }
    
}