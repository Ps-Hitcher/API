using WebApplication2.Models.Travel;

namespace WebApplication2.Models;

public class MyTrips
{
    public List<ChosenTripsModel> myTrips { get; set; }
    public List<TravelModel> travels { get; set; }
    public string LatLng { get; set; }
}