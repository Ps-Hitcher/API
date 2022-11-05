using Newtonsoft.Json;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using WebApplication2.Utilities;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using FileIO = System.IO.File;

namespace WebApplication2.Models.Travel;
public class TravelRepository : ITravelRepository
{
    private DbSet<TravelModel> TravelList;
    public DataContext _context;
    public const string _jsonPath = "TravelDB.json";
    public TravelRepository(DataContext context)
    {
        _context = context;
        TravelList = context.Trips;
    }
    public TravelModel GetTravel(Guid Id)
    {
        return TravelList.FirstOrDefault(e => e.Id == Id);
    }
    public DbSet<TravelModel> GetTravelList()
    {
        return TravelList;
    }
}