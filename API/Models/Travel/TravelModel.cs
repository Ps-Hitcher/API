using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Models.Travel;

public class TravelModel
{
    [Required]
    [Key]
    [ForeignKey("MetaModel")]
    public Guid Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Origin { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Destination { get; set; }
    
    public List<string>? Stopovers { get; set; }
    
    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm}", ApplyFormatInEditMode = true)]
    public DateTime Time { get; set; }
    
    [Required]
    public Guid DriverID { get; set; }
    
    [Required]
    public int FreeSeats { get; set; }
    
    [StringLength(1000)]
    public string? Description { get; set; }
    
    [Required]
    public string RequestInfo { get; set; }
    
}
