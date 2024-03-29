﻿using System;
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
using ITravelRepository = WebApplication2.Models.ITravelRepository;

namespace DemoTest;

public class HomeControllerTests
{
    [Test]
    public void AddUser_ShouldRetunOkResponse_WhenDataFound()
    {
        var _contextOptions = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new DataContext(_contextOptions);

        var record1 = new UserModel
        {
            Name = "Kamile",
            Surname = "Samusiovaite",
            YearOfBirth = "2002-02-12",
            PhoneNumber = "860569664",
            Address = "Ukmerge",
            Description = "Trenere",
            Email = "kamile@gmail.com"
        };

        context.Database.EnsureCreated();
        context.SaveChanges();

        var MockUserModel = new Mock<UserModel>();
        var MockILogger = new Mock<ILogger<HomeController>>();
        var MockIDGenerator = new Mock<CorrelationIdGenerator>();
        var MockIErrrorRepository = new Mock<IErrorRepository>();
        var MockIUserRepository = new Mock<IUserRepository>(); 
        var MockDbSetUser = new Mock<DbSet<UserModel>>();
        var MockITravelRepository = new Mock<ITravelRepository>();
        var MockDbSetTravel = new Mock<DbSet<TravelModel>>();
        var MockMetaRepository = new Mock<IMetaRepository>();
        var MockIMetaRepository = new Mock<DbSet<MetaModel>>();

        var MockCoreelationIDGenerator = new Mock<ICorrelationIDGenerator>();
        var MockErrorRepository = new Mock<IErrorRepository>();

        DbSet<UserModel> set = context.Set<UserModel>();
        DbSet<TravelModel> set1 = context.Set<TravelModel>();

        MockIUserRepository.Setup(user1 => user1.GetUserList()).Returns(set); 
        MockITravelRepository.Setup(user1 => user1.GetTravelList()).Returns(set1);
        MockIUserRepository.Setup(user1 => user1.IsValidPhone(record1.PhoneNumber)).Returns(true);
        
        var homeController = new HomeController(MockILogger.Object, MockIUserRepository.Object, MockITravelRepository.Object, MockMetaRepository.Object,  MockIErrrorRepository.Object, MockIDGenerator.Object, context);

        homeController.AddUser(record1);

        var result = context.Users.Find(record1.Id);
        Assert.AreEqual(result, record1);
    }
 
    [Test]
    public void Users_ShouldReturnUsersView()
    {
        
        var _contextOptions = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new DataContext(_contextOptions);
        var MockILogger = new Mock<ILogger<HomeController>>();
        var MockIDGenerator = new Mock<CorrelationIdGenerator>();
        var MockIErrrorRepository = new Mock<IErrorRepository>();
        var MockIUserRepository = new Mock<IUserRepository>();
        var MockITravelRepository = new Mock<ITravelRepository>();
        var MockMetaRepository = new Mock<IMetaRepository>();
        
        var homeController = new HomeController(MockILogger.Object, MockIUserRepository.Object,
            MockITravelRepository.Object, MockMetaRepository.Object,
            MockIErrrorRepository.Object, MockIDGenerator.Object, context);

        var actResult = homeController.Users() as ViewResult;
        Assert.That(actResult.ViewName, Is.EqualTo("Users"));
    }
}