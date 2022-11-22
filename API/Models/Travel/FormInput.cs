namespace WebApplication2.Models.Travel;

public class FormInput
{
    public string Origin { get; set; }

    public string Destination { get; set; }
    
    public List<string>? Stopovers { get; set; }
    
    public DateTime Time { get; set; }

    public Guid DriverID { get; set; }

    public int FreeSeats { get; set; }

    public string? Description { get; set; }
    
    public List<string> Bearings { get; set; }

    public List<string> Distance { get; set; }
    
    public List<double> lat { get; set; }

    public List<double> lon { get; set; }
}