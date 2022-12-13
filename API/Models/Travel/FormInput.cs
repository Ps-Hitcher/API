namespace WebApplication2.Models.Travel;

public class FormInput
{
    public string Origin { get; set; }
    public string Destination { get; set; }
    public string Stopovers { get; set; }
    public DateTime Time { get; set; }
    public Guid DriverID { get; set; }
    public int FreeSeats { get; set; }
    public string? Description { get; set; }
    public string Bearings { get; set; }
    public string Distance { get; set; }
    public string Lat { get; set; }
    public string Lng { get; set; }
}