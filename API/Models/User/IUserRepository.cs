namespace WebApplication2.Models.User;
using Microsoft.EntityFrameworkCore;
public interface IUserRepository
{
    UserModel GetUser(Guid Id);
    DbSet<UserModel> GetUserList();
    public bool IsValidPhone(string PhoneNumber);
    public void DeleteUser(Guid Id);
    public void Save();
}