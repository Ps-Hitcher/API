﻿namespace WebApplication2.Models.User;

public interface IUserRepository
{
    UserModel GetUser(Guid Id);
    List<UserModel> GetUserList();

    public bool IsValidPhone(string PhoneNumber);
}