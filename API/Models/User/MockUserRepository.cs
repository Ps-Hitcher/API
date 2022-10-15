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
            Debug.WriteLine(ex.Message);
            throw;
        }

        //_userList = new List<UserModel>()
        //{
        //    new UserModel(){Id = Guid.NewGuid(), Type = UserType.Driver, Name = "Domas", Surname = "Nemanius", Email = "Domas@gmail.com", PhoneNumber = "+37063666660", CarId = null},
        //    new UserModel(){Id = Guid.NewGuid(), Type = UserType.User, Name = "Adomas", Surname = "Vensas", Email = "Adomas@gmail.com", PhoneNumber = "+37063666661", CarId = null},
        //    new UserModel(){Id = Guid.NewGuid(), Type = UserType.User, Name = "Kamile", Surname = "Samusiovaite", Email = "Kamile@gmail.com", PhoneNumber = "+37063666662", CarId = null},
        //    new UserModel(){Id = Guid.NewGuid(), Type = UserType.User, Name = "Andrius", Surname = "Paulauskas", Email = "Andrius@gmail.com", PhoneNumber = "+37063666663", CarId = null},
        //};
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