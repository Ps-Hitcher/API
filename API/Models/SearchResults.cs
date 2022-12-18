using WebApplication2.Models.Travel;
using WebApplication2.Models.User;

namespace WebApplication2.Models;

public class SearchResults
{
    public string? Origin { get; set; }
    public double? OriginLat { get; set; }
    public double? OriginLng { get; set; }
    public string? Destination { get; set; }
    public double? DestinationLat { get; set; }
    public double? DestinationLng { get; set; }
    public double? Bearings { get; set; }
    public string TravelId { get; set; }
    public TravelUser TravelUser { get; set; }

    public SearchResults()
    {
        this.TravelUser = new TravelUser();
    }
}