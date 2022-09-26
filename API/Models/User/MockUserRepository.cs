namespace WebApplication2.Models.User;

public class MockUserRepository : IUserRepository
{
    private List<UserModel> _userList;

    public MockUserRepository()
    {
        _userList = new List<UserModel>()
        {
            new UserModel(){Id = Guid.NewGuid(), UserType = 1, Name = "Domas", Surname = "Nemanius", Email = "Domas@gmail.com", PhoneNumber = "+37063666660", CarId = null},
            new UserModel(){Id = Guid.NewGuid(), UserType = 1, Name = "Adomas", Surname = "Vensas", Email = "Adomas@gmail.com", PhoneNumber = "+37063666661", CarId = null},
            new UserModel(){Id = Guid.NewGuid(), UserType = 1, Name = "Kamile", Surname = "Samusiovaite", Email = "Kamile@gmail.com", PhoneNumber = "+37063666662", CarId = null},
            new UserModel(){Id = Guid.NewGuid(), UserType = 1, Name = "Andrius", Surname = "Paulauskas", Email = "Andrius@gmail.com", PhoneNumber = "+37063666663", CarId = null},
        };
    }

    public UserModel GetUser(Guid Id)
    {
        return _userList.FirstOrDefault(e => e.Id == Id);
    }
}