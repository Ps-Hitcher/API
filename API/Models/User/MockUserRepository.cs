using Newtonsoft.Json;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using WebApplication2.Utilities;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using FileIO = System.IO.File;



namespace WebApplication2.Models.User;

public class MockUserRepository : IUserRepository
{
    private DbSet<UserModel> _userList;
    public DataContext _context;
    public MockUserRepository(DataContext context)
    {
        _context = context;
        _userList = context.Users;
    }

    public UserModel GetUser(Guid Id)
    {
        return _userList.FirstOrDefault(e => e.Id == Id);
    }

    public void Save()
    {
        _context.SaveChanges();
    }

    public DbSet<UserModel> GetUserList()
    {
        return _userList;
    }
    
    public bool IsValidPhone(string PhoneNumber)
    {
        if (string.IsNullOrEmpty(PhoneNumber))
            return false;
        var r = new Regex(@"^\(?(86|\+3706)?[-.●]?([0-9]{3})[-.●]?([0-9]{4})$");
        return r.IsMatch(PhoneNumber);
           
    }
    public void DeleteUser(Guid Id)
    {
        _userList.Remove(GetUser(Id));
    }

    public static int GetUserAge (string YearOfBirth)
    {
        DateTime today = DateTime.Today;
        int age = today.Year - DateTime.Parse(YearOfBirth).Year;
        if (DateTime.Parse(YearOfBirth) > today.AddYears(-age)) age--;

        return age;
    }
    
}