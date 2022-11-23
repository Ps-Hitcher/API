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

namespace DemoTest;

public class MockUserRepositoryTest
{
    private UserModel _user1;
    private UserModel _user2;
    private IList<UserModel> _users;
    

    [SetUp]
    public void Setup()
    {
        _user1 = new UserModel() { Name = "Kamile", Surname = "Samusiovaite", YearOfBirth = "2002-02-12", PhoneNumber = "860569664"};
        _user2 = new UserModel() { Name = "Domas", Surname = "Nemanius", YearOfBirth = "2023-09-12" };
        _users = new List<UserModel> { _user1, _user2 };
    }

    [Test]
    public void GetUserAge_ShouldRetunOkResponse_WhenReturnsTheRightAnswer()
    {
        Assert.AreEqual(MockUserRepository.GetUserAge((_user1.YearOfBirth)), 20);
        Assert.AreEqual(MockUserRepository.GetUserAge((_user2.YearOfBirth)), -1);
        Assert.AreEqual(MockUserRepository.GetUserAge("2002-12-12"), 19);
        
    }
 
    [Test]
    public void IsValidPhone_ShouldReturnOkResponse_WhenReturnsTheRightAnswer()
    {
        var _contextOptions = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new DataContext(_contextOptions);

        var record1 = new UserModel
        {
            Name = "Audrele",
            Surname = "Koviniene",
            YearOfBirth = "1982-08-22",
            PhoneNumber = "868273625",
            Address = "Augunu g.9",
            Description = "Taksistas",
            Email = "Audrele.Kovine@gmail.com"
        };
        
        var record2 = new UserModel
        {
            Name = "Jonas",
            Surname = "Makalis",
            YearOfBirth = "2001-01-19",
            PhoneNumber = "868293827",
            Address = "Jonu g.2",
            Description = "Kosmetologas",
            Email = "Jonas.Makalelis@gmail.com"
        };

        context.Database.EnsureCreated();
        context.AddRange(record1);
        context.SaveChanges();
        
        var mockUserRepository = new MockUserRepository(context);
        mockUserRepository.IsValidPhone(record1.PhoneNumber);
        
        Assert.AreEqual(mockUserRepository.IsValidPhone(_user1.PhoneNumber),true);
        Assert.AreEqual(mockUserRepository.IsValidPhone(_user2.PhoneNumber),false);
    }

    [Test]
    public void GetUser_ShouldReturnOkResponse_WhenGetUser()
    {
        var _contextOptions1 = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new DataContext(_contextOptions1);

        var record1 = new UserModel
        {
            Id = Guid.Parse("86cb5f75-80f0-410e-9ab6-26ed5bdb7ff4"),
            Name = "Antanas",
            Surname = "Lipkauskas",
            YearOfBirth = "2002-01-19",
            PhoneNumber = "861720382",
            Address = "Liepu g.2",
            Description = "Statybininkas",
            Email = "Antanas.Lipkauskas@gmail.com"
        };
        
        var record2 = new UserModel
        {
            Id = Guid.Parse("86cb5f75-80f0-410e-9ab6-26ed5bdb7ff5"),
            Name = "Lukas",
            Surname = "Sinkevic",
            YearOfBirth = "1992-01-19",
            PhoneNumber = "86292",
            Address = "Jonu g.2",
            Description = "Vairuotojas",
            Email = "Lukas.Sink@gmail.com"
        };

        context.Database.EnsureCreated();
        context.AddRange(record1);
        context.SaveChanges();
        
        var mockUserRepository = new MockUserRepository(context);
        mockUserRepository.GetUser(record1.Id);
        
        var result1 = context.Users.Find(record1.Id);
        var result2 = context.Users.Find(record2.Id);
        
        Assert.AreEqual(mockUserRepository.GetUser(record1.Id),result1);
        Assert.AreEqual(mockUserRepository.GetUser(record2.Id),result2);
    }
    
    [Test]
    public void Delete_ShouldReturnOkResponse_WhenDataDelete()
    {
        var _contextOptions1 = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new DataContext(_contextOptions1);

        var record1 = new UserModel
        {
            Id = Guid.Parse("86cb5f75-80f0-410e-9ab6-26ed5bdb7ff5"),
            Name = "Gabija",
            Surname = "Daunoraite",
            YearOfBirth = "2002-10-12",
            PhoneNumber = "867283992",
            Address = "Jonava",
            Description = "Kirpeja",
            Email = "Gabijuks@gmail.com"
        };

        context.Database.EnsureCreated();
        context.AddRange(record1);
        context.SaveChanges();
        
        var mockUserRepository = new MockUserRepository(context);
        mockUserRepository.DeleteUser(record1.Id);
        mockUserRepository.Save();
        var result  = context.Users.Any();
        Assert.False(result);
    }
    
    [Test]
    public void GetUserList_ShouldReturnOkResponse_WhenGetUserList()
    {
        var _contextOptions = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new DataContext(_contextOptions);
        
        var record1 = new UserModel
        {
            Id = Guid.Parse("86cb5f75-80f0-410e-9ab6-26ed5bdb7ff5"),
            Name = "Kamile",
            Surname = "Samusiovaite",
            YearOfBirth = "2002-02-12",
            PhoneNumber = "860569664",
            Address = "Ukmerge",
            Description = "vaikas",
            Email = "kamile@gmail.com"
        };
        var record2 = new UserModel
        {
            Id = Guid.Parse("86cb5f75-80f0-410e-9ab6-26ed5bdb7ff4"),
            Name = "Domas",
            Surname = "Nemanius",
            YearOfBirth = "1992-02-12",
            PhoneNumber = "860569882",
            Address = "Šiauliai",
            Description = "Pirikupas",
            Email = "Domas.Nemaniuks@gmail.com"
        };

        context.Database.EnsureCreated();
        context.AddRange(record1);
        context.AddRange(record2);
        context.SaveChanges();
        
        var mockUserRepository = new MockUserRepository(context);
        var result = mockUserRepository.GetUserList();

        result.Remove(record1);
        result.Remove(record2);
        context.SaveChanges();

        var answer  = context.Users.Any();
        Assert.False(answer);
        
    }
    
}