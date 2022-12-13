using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace WebApplication2.Models.User;

public class UserModel : IComparable<UserModel>
{
    public Guid Id { get; set; }
    public UserType? Type { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? YearOfBirth { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? Description { get; set; }

    public string? Password { get; set; }
    
    //public CarStruct? Car { get; set; }
    public Double? Rating { get; set; }

    public String? CarModel { get; set; }
    
    public String? CarNumber { get; set; }
    
    public Double? Fuel { get; set; }



    public int CompareTo(UserModel? other)
    {
        if ((float)this.Rating <  (float)other.Rating) return -1;
        if ((float)this.Rating == (float)other.Rating) return 0;
        return 1;
    }
}