using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models.Travel;

public class CoordsModel
{
    [Required]
    public Guid MetaId { get; set; }
    [Required]
    public int position { get; set; }
    [Required]
    public double lat { get; set; }
    [Required]
    public double lon { get; set; }
}