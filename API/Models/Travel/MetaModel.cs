using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace WebApplication2.Models.Travel;

public class MetaModel
{  
    [Required]
    public Guid TravelId { get; set; }
    [Required]
    [StringLength(100)]
    public String MetaDestination { get; set; }
    public double Bearing { get; set; }
    public double Distance { get; set; }
}
