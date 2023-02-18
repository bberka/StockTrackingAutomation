using Domain.Entities;
using Domain.Models;
using EasMe.Models;

namespace Domain.Abstract;

public interface IUserService
{
    ResultData<User> Login(LoginModel model);
    Result Register(User user);
    Result UpdateUser(User user);
    List<User> GetList();
    Result DeleteUser(int id);
    ResultData<User> GetUser(int id);
    Result ChangePassword(int userId, ChangePasswordModel model);
}