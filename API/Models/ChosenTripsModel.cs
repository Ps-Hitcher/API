using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models;

public class ChosenTripsModel
{
    [Required, Key, ForeignKey("UserModel")]
    public Guid UserId { get; set; }
    [Required, Key, ForeignKey("TravelModel")]
    public Guid TravelId { get; set; }
    [Required]
    public bool Driver { get; set; }
}