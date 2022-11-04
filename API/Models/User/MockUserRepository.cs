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
    //private List<UserModel> _userList;
    private DbSet<UserModel> _userList;
    public const string _jsonPath = "db.json";
    public DataContext _context;
    public MockUserRepository(DataContext context)
    {
        // try
        // {
        //     _userList = JsonConvertUtil.DesirializeJSON<List<UserModel>>(_jsonPath);
        // }
        // catch (Exception ex)
        // {
        //     Debug.WriteLine(ex.ToString());
        //     ErrorLogUtil.LogError(ex);
        // }
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

    // public List<UserModel> GetUserList()
    // {
    //     return _userList;
    // }
    
    public DbSet<UserModel> GetUserList()
    {
        return _userList;
    }
    
    public bool IsValidPhone(string PhoneNumber)
    {
        try
        {
            if (string.IsNullOrEmpty(PhoneNumber))
                return false;
            var r = new Regex(@"^\(?(86|\+3706)?[-.●]?([0-9]{3})[-.●]?([0-9]{4})$");
            return r.IsMatch(PhoneNumber);
           
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            ErrorLogUtil.LogError(ex);
            return false;
        }
    }

    public void DeleteUser(Guid Id)
    {
        _userList.Remove(GetUser(Id));
    }

    public void SerializeUserList(List<UserModel> UserList)
    {
        try
        {
            //throw new Exception("Error here");  //For presentation
            string? jsonData = null;
            FileIO.WriteAllText(_jsonPath, jsonData.SerializeJSON(UserList));
        }
        catch(Exception ex)
        {
            Debug.WriteLine(ex.Message);
            ErrorLogUtil.LogError(ex, "Adomas is responsible for this mess");
        }
    }
}