using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models.Travel;

public class MetaModel
{  
    [Required, Key]
    public Guid TravelId { get; set; }
    [Required, StringLength(100)]
    public String Origin { get; set; }
    public double OriginLat { get; set; }
    public double OriginLng { get; set; }
    [Required, Key, StringLength(100)]
    public string Destination { get; set; } = null!;

    public double DestinationLat { get; set; }
    public double DestinationLng { get; set; }
    public double Bearing { get; set; }
    public double Distance { get; set; }
}
