using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Models.Travel;

public class TravelModel
{
    [Required, Key, ForeignKey("MetaModel")]
    public Guid Id { get; set; }
    
    [Required, StringLength(100), Display(Name = "Origin")]
    public string Origin { get; set; }
    
    [Required, StringLength(100), Display(Name = "Destination")]
    public string Destination { get; set; }
    
    [Display(Name = "Origin")]
    public List<string>? Stopovers { get; set; }
    
    [Required, DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm}", ApplyFormatInEditMode = true), Display(Name = "Leave time")]
    public DateTime Time { get; set; }
    
    [Required]
    public Guid DriverId { get; set; }
    public string DriverName { get; set; }
    public string DriverSurname { get; set; }
    
    [Required, Display(Name = "Free seats left")]
    public int FreeSeats { get; set; }
    
    [StringLength(1000), Display(Name = "Description")]
    public string? Description { get; set; }
    
}
