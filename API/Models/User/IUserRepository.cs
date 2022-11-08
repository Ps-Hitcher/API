namespace WebApplication2.Models.User;
using Microsoft.EntityFrameworkCore;
public interface IUserRepository
{
    UserModel GetUser(Guid Id);
    DbSet<UserModel> GetUserList();
    public bool IsValidPhone(string PhoneNumber);
    public void DeleteUser(Guid Id);
    public void Save();
    public static int GetUserAge(string YearOfBirth){
        DateTime today = DateTime.Today;
        int age = today.Year - DateTime.Parse(YearOfBirth).Year;
        if (DateTime.Parse(YearOfBirth) > today.AddYears(-age)) age--;

        return age;
    }
}