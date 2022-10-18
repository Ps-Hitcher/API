namespace WebApplication2.Models.Travel;

public class TravelModel
{
    public Guid TravelId { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }
    public List<string> Stopovers { get; set; }
    public string LeaveTime { get; set; }
    public Guid DriverID { get; set; }
    public int FreeSeats { get; set; }
    public string Description { get; set; }
}
