namespace WebApplication2.Models.Travel;

public class SearchTravel
{
    public string? Origin { get; set; }
    public double? OriginLat { get; set; }
    public double? OriginLng { get; set; }
    public string? Destination { get; set; }
    public double? DestinationLat { get; set; }
    public double? DestinationLng { get; set; }
    public double? Bearings { get; set; }
    public TravelModel? TravelModel { get; set; }

    public SearchTravel()
    {
        this.TravelModel = new TravelModel();
    }
    public SearchTravel(TravelModel t)
    {
        this.TravelModel = t;
    }
}