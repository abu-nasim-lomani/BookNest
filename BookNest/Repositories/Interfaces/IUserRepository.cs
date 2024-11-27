using BookNest.Models;
using System.Collections.Generic;

namespace BookNest.Repositories.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUsers();
        User GetUserById(string userId);
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(string userId);
    }
}
