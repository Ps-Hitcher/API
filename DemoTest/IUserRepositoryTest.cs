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
        _user1 = new UserModel()
            { Name = "Kamile", Surname = "Samusiovaite", YearOfBirth = "2002-02-12", PhoneNumber = "860569664" };
        _user2 = new UserModel() { Name = "Domas", Surname = "Nemanius", YearOfBirth = "2023-09-12" };
    }

    [Test]
    public void GetUserAge_ShouldRetunOkResponse_WhenReturnsTheRightAnswer()
    {
        Assert.AreEqual(IUserRepository.GetUserAge((_user1.YearOfBirth)), 20);
        Assert.AreEqual(IUserRepository.GetUserAge((_user2.YearOfBirth)), -1);
        Assert.AreEqual(IUserRepository.GetUserAge("2002-12-12"), 19);

    }
}