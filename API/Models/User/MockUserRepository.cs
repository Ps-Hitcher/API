using Newtonsoft.Json;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace WebApplication2.Models.User;

public class MockUserRepository : IUserRepository
{
    private List<UserModel> _userList = null;
    private const string _jsonPath = "db.json";
    public MockUserRepository()
    {
        _userList = DesirializeJSON<List<UserModel>>(_jsonPath);

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

    /// <summary>
    /// Intermediary function to retrieve JSON data to _userList
    /// </summary>
    //private async void GetJSONData()
    //{
        //var task = Task.Factory.StartNew(() => DesirializeJSON<List<UserModel>>(_jsonPath));

        //Makes async useless but currently the only way it works
        //Task.WaitAll(task);
        //_userList = await task;

    //}

    /// <summary>
    /// Desirializes a JSON file to a desirable type
    /// </summary>
    /// <typeparam name="T">A desirable type</typeparam>
    /// <param name="jsonPath">Path were the JSON file is located</param>
    /// <returns>A specified type's object with desirialized JSON data</returns>
    private T DesirializeJSON<T>(string jsonPath)
    {
        T? tempList = default(T);
        try
        {
            using (StreamReader r = new StreamReader(jsonPath))
            {
                var json = r.ReadToEnd();
                tempList = JsonConvert.DeserializeObject<T>(json);
            }
            return tempList;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return tempList;
        }


        //AN OPTION WITHOUT NEWTONSOFT
        //JsonSerializer serializer = new JsonSerializer();
        //using (StreamReader r = new StreamReader(_jsonPath))
        //{
        //    using (JsonTextReader reader = new JsonTextReader(r))
        //    {
        //        _userList = serializer.Deserialize<List<UserModel>>(reader);
        //    }
        //}


    }
}