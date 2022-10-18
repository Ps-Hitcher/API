using Newtonsoft.Json;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using WebApplication2.Utilities;
using System.Text.RegularExpressions;

namespace WebApplication2.Models.Travel;
public class TravelRepository : ITravelRepository
{
    private List<TravelModel> TravelList;
    public const string _jsonPath = "TravelDB.json";
    public TravelRepository()
    {
        try
        {
            TravelList = JsonConvertUtil.DesirializeJSON<List<TravelModel>>(_jsonPath);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            ErrorLogUtil.LogError(ex);
        }
    }

    public TravelModel GetTravel(Guid Id)
    {
        return TravelList.FirstOrDefault(e => e.TravelId == Id);
    }

    public List<TravelModel> GetTravelList()
    {
        return TravelList;
    }
}