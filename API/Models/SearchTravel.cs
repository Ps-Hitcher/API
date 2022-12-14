using WebApplication2.Models.Travel;

namespace WebApplication2.Models;

public class SearchTravel
{
    public string? Origin { get; set; }
    public double? OriginLat { get; set; }
    public double? OriginLng { get; set; }
    public string? Destination { get; set; }
    public double? DestinationLat { get; set; }
    public double? DestinationLng { get; set; }
    public double? Bearings { get; set; }
    public List<TravelModel>? TravelResults { get; set; }

    public SearchTravel()
    {
        this.TravelResults = new List<TravelModel>();
    }
    public SearchTravel(TravelModel t)
    {
        this.TravelResults = new List<TravelModel>();
        this.TravelResults.Add(t);
    }
}