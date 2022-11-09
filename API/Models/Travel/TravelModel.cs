using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Models.Travel;

public class TravelModel
{
    public Guid Id { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }
    public List<string>? Stopovers { get; set; }
    public String Time { get; set; }
    public Guid DriverID { get; set; }
    public int FreeSeats { get; set; }
    public string? Description { get; set; }
    public string RequestInfo { get; set; }
    public List<double>? Bearings { get; set; }
}
