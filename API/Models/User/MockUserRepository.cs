using Newtonsoft.Json;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using WebApplication2.Utilities;
using System.Text.RegularExpressions;


namespace WebApplication2.Models.User;

public class MockUserRepository : IUserRepository
{
    private List<UserModel> _userList;
    public const string _jsonPath = "db.json";
    public MockUserRepository()
    {
        try
        {
            _userList = JsonConvertUtil.DesirializeJSON<List<UserModel>>(_jsonPath);
        }
        catch (Exception ex)
        {
            //TODO: A text file in which all errors are logged
            Debug.WriteLine(ex.ToString());
            throw;
        }
    }

    public UserModel GetUser(Guid Id)
    {
        return _userList.FirstOrDefault(e => e.Id == Id);
    }

    public List<UserModel> GetUserList()
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
        catch (Exception)
        {
            throw;
        }
    }
}