using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models.Travel;

public class MetaModel
{  
    [Required]
    public Guid TravelId { get; set; }
    [Required]
    [StringLength(100)]
    public String Origin { get; set; }
    [Required]
    public double OriginLat { get; set; }
    [Required]
    public double OriginLng { get; set; }
    [Required]
    [StringLength(100)]
    public String Destination { get; set; }
    [Required]
    public double DestinationLat { get; set; }
    [Required]
    public double DestinationLng { get; set; }
    public double Bearing { get; set; }
    public double Distance { get; set; }
}
