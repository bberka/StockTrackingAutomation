using Domain.Entities;
using Domain.Models;
using EasMe.Models;

namespace Domain.Abstract;

public interface IUserService
{
    ResultData<User> Login(LoginModel model);
    Result Register(User user);
    Result UpdateUser(User user);
    List<User> GetValidUsers();
    Result DeleteUser(int id);
    User? GetUser(int id);
}