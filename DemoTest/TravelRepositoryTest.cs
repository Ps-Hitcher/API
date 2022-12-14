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

public class TravelRepositoryTest
{
    [Test]
    public void GetTravel_ShouldReturnOkResponse_WhenGetTravel()
    {
        var _contextOptions = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new DataContext(_contextOptions);

        var record1 = new TravelModel
        {
            Id = Guid.Parse("da7bb42a-c0d7-461e-a198-158f5585de56"),
            Origin = "Akropolis",
            Destination = "Šeškinės poliklinika",
            Stopovers = new List<String>{"Alytus, Alytus City Municipality, Lithuania"},
            Time = new DateTime(1900, 01, 01, 02, 45, 00, 00),
            DriverId = Guid.Parse("86cb5f75-80f0-410e-9ab6-26ed5bdb7ff4"),
            FreeSeats = 2,
            Description = "kelione1",
            DriverName = "Domas",
            DriverSurname = "Nemanius"
        };
        
        var record2 = new TravelModel
        {
            Id = Guid.Parse("60a12766-1052-41f8-b4a3-3c6fa407bc72"),
            Origin = "Ukmergė",
            Destination = "Kaunas",
            Stopovers = new List<String>{"Vilnius,Kaunas"},
            Time = new DateTime(2022, 11, 29, 01, 06, 00, 00),
            DriverId = Guid.Parse("00000000-0000-0000-0000-000000000000"),
            FreeSeats = 2,
            Description = "kelione2",
            DriverName = "Domas",
            DriverSurname = "Nemanius"
        };

        context.Database.EnsureCreated();
        context.AddRange(record1);
        context.AddRange(record2);
        context.SaveChanges();

        var travelRepository = new TravelRepository(context);
        travelRepository.GetTravel(record1.Id);
        
        var result1 = context.Trips.Find(record1.Id);
        var result2 = context.Trips.Find(record2.Id);
        
        Assert.AreEqual(travelRepository.GetTravel(record1.Id),result1);
        Assert.AreEqual(travelRepository.GetTravel(record2.Id),result2);
    }

    [Test]
    public void GetTravelList_ShouldReturnOkResponse_WhenGetUserList()
    {
        var _contextOptions = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new DataContext(_contextOptions);
        var record1 = new TravelModel
        {
            Id = Guid.Parse("da7bb42a-c0d7-461e-a198-158f5585de56"),
            Origin = "Akropolis",
            Destination = "Šeškinės poliklinika",
            Stopovers = new List<String>{"Alytus, Alytus City Municipality, Lithuania"},
            Time = new DateTime(1900, 01, 01, 02, 45, 00, 00),
            DriverId = Guid.Parse("00000000-0000-0000-0000-000000000000"),
            FreeSeats = 2,
            Description = "kelione1",
            DriverName = "Domas",
            DriverSurname = "Nemanius"
        };
        
        var record2 = new TravelModel
        {
            Id = Guid.Parse("60a12766-1052-41f8-b4a3-3c6fa407bc72"),
            Origin = "Ukmergė",
            Destination = "Kaunas",
            Stopovers = new List<String>{"Vilnius,Kaunas"},
            Time = new DateTime(2022, 11, 29, 01, 06, 00, 00),
            DriverId = Guid.Parse("00000000-0000-0000-0000-000000000000"),
            FreeSeats = 2,
            Description = "kelione2",
            DriverName = "Domas",
            DriverSurname = "Nemanius"
        };

        context.Database.EnsureCreated();
        context.AddRange(record1);
        context.AddRange(record2);
        context.SaveChanges();
        
        var travelRepository = new TravelRepository(context);
        var result = travelRepository.GetTravelList();

        result.Remove(record1);
        result.Remove(record2);
        context.SaveChanges();

        var answer  = context.Users.Any();
        Assert.False(answer);
    }
    
}