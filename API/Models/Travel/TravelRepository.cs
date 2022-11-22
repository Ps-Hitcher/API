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
    public TravelRepository(DataContext context)
    {
        _context = context;
        TravelList = context.Trips;
    }
    public TravelModel GetTravel(Guid id)
    {
        return TravelList.FirstOrDefault(e => e.Id == id) ?? throw new InvalidOperationException();
    }
    public void Save()
    {
        _context.SaveChanges();
    }
    public DbSet<TravelModel> GetTravelList()
    {
        return TravelList;
    }
}