using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models.Travel;

public class TravelModel
{
    public Guid Id { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }
    
    public List<StopoverModel> Stopovers { get; set; }
    public String Time { get; set; }
    public Guid DriverID { get; set; }
    public int FreeSeats { get; set; }
    public string Description { get; set; }
}
