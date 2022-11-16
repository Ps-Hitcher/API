using WebApplication2.Models.Travel;
using HtmlAgilityPack;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Utilities
{
    public static class CreateTripsUtil
    {
        public static async Task GenerateTrips(int amount, DbSet<TravelModel> travelModels)
        {
            string url = "https://www.generatormix.com/random-address-in-lithuania?number=" + amount;
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync(url);
            HtmlDocument htmlDocument = new();

            htmlDocument.LoadHtml(response);

            var paragraphList = htmlDocument.DocumentNode.Descendants("p").Where(x => x.GetAttributeValue("class", "") == "text-left").ToList();
            var streetNames = paragraphList.Where(x => x.InnerText.Contains("Street")).Select(x => x.InnerText[8..]).ToList();
            var countyNames = paragraphList.Where(x => x.InnerText.Contains("County/Department")).Select(x => x.InnerText[27..]).ToList();
            var cityNames = paragraphList.Where(x => x.InnerText.Contains("Suburb/City")).Select(x => x.InnerText[13..]).ToList();
            var stateRegion = paragraphList.Where(x => x.InnerText.Contains("State/Region")).Select(x => x.InnerText[14..]).ToList();

            for (int i = 0; i < amount; ++i)
            {
                var travelId = Guid.NewGuid();
                var driverId = Guid.NewGuid();
                var freeSeats = Random.Shared.Next(1, 5);
                var hours = Random.Shared.Next(0, 25);
                var minutes = Random.Shared.Next(0, 61);
                var origin = $"Lithuania {cityNames.RandomElement()} {streetNames.RandomElement()}";
                var destination = $"Lithuania {cityNames.RandomElement()} {streetNames.RandomElement()}";

                while(origin.Length > 56)
                    origin = $"Lithuania {cityNames.RandomElement()} {streetNames.RandomElement()}";
                while(destination.Length > 56)
                    destination = $"Lithuania {cityNames.RandomElement()} {streetNames.RandomElement()}";


                travelModels.Add(
                    new TravelModel() {
                        Id = travelId,
                        DriverID = driverId,
                        FreeSeats = freeSeats,
                        Time = $"{hours}:{minutes}",
                        Origin = origin,
                        Destination = destination,
                        RequestInfo = "Geodata"
                    }
                );
            }
        }

        public static T RandomElement<T>(this IEnumerable<T> list)
        {
            var index = Random.Shared.Next(0, list.Count());
            return list.ElementAt(index);
        }


    }
}
