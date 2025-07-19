using AvaloniaApp.Models;
using System.Collections.Generic;

namespace AvaloniaApp.ServiceAbstractions
{
    public interface IUserService
    {
        void CreateUser(User user);
        bool DeleteUser(int id);
        List<User> GetAllUsers();
        User? GetUserById(int id);
        bool UpdateUser(User user);
    }
}