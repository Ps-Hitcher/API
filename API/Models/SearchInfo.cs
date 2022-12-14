namespace WebApplication2.Models.Travel;

public class SearchInfo
{
    public string? Origin { get; set; }
    public double? OriginLat { get; set; }
    public double? OriginLng { get; set; }
    public string? Destination { get; set; }
    public double? DestinationLat { get; set; }
    public double? DestinationLng { get; set; }
    public double? Bearings { get; set; }
}