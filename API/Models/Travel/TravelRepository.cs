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
    // private List<TravelModel> TravelList;
    private DbSet<TravelModel> TravelList;
    public DataContext _context;
    public const string _jsonPath = "TravelDB.json";
    public TravelRepository(DataContext context)
    {
        // try
        // {
        //     TravelList = JsonConvertUtil.DesirializeJSON<List<TravelModel>>(_jsonPath);
        // }
        // catch (Exception ex)
        // {
        //     Debug.WriteLine(ex.ToString());
        //     ErrorLogUtil.LogError(ex);
        // }
        _context = context;
        TravelList = context.Trips;
    }

    public TravelModel GetTravel(Guid Id)
    {
        return TravelList.FirstOrDefault(e => e.Id == Id);
    }

    // public List<TravelModel> GetTravelList()
    // {
    //     return TravelList;
    // }
    public DbSet<TravelModel> GetTravelList()
    {
        return TravelList;
    }

    public void SerializeTravelList(List<TravelModel> TravelList)
    {
        try
        {
            //throw new Exception("Error here");  //For presentation
            string? jsonData = null;
            FileIO.WriteAllText(_jsonPath, jsonData.SerializeJSON(TravelList));
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            ErrorLogUtil.LogError(ex, "Adomas is responsible for this mess too");
        }
    }
}