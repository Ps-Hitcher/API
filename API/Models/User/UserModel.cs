namespace WebApplication2.Models.User;

public class UserModel
{
    public Guid Id { get; set; }
    public int UserType { get; set; }//enum
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public Guid?  CarId{ get; set; }
    
}