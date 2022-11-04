namespace WebApplication2.Models.User;
using Microsoft.EntityFrameworkCore;
public interface IUserRepository
{
    UserModel GetUser(Guid Id);
    // List<UserModel> GetUserList();
    DbSet<UserModel> GetUserList();
    public bool IsValidPhone(string PhoneNumber);
    public void DeleteUser(Guid Id);
    public void Save();

    public void SerializeUserList(List<UserModel> UserList);
}