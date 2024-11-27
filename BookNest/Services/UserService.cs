using System;
using System.Collections.Generic;
using System.Linq;
using BookNest.Models;
using BookNest.Repositories.Interfaces;

namespace BookNest.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IBookIssueRepository _bookIssueRepository;

        public UserService(IUserRepository userRepository, IBookIssueRepository bookIssueRepository)
        {
            _userRepository = userRepository;
            _bookIssueRepository = bookIssueRepository;
        }

        public User GetUserById(string userId)
        {
            return _userRepository.GetUserById(userId);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

        public void AddUser(User user)
        {
            _userRepository.AddUser(user);
        }

        public void UpdateUser(User user)
        {
            _userRepository.UpdateUser(user);
        }

        public void DeleteUser(string userId)
        {
            _userRepository.DeleteUser(userId);
        }

        public void CheckAndRestrictUsers()
        {
            var overdueIssues = _bookIssueRepository.GetAllBookIssues().Where(bi => !bi.IsReturned && bi.ReturnDate < DateTime.Now);

            foreach (var issue in overdueIssues)
            {
                var user = _userRepository.GetUserById(issue.UserId);
                if (user != null && !user.IsRestricted)
                {
                    user.IsRestricted = true;
                    _userRepository.UpdateUser(user);
                }
            }
        }

        public void LiftRestriction(string userId)
        {
            var user = _userRepository.GetUserById(userId);
            if (user != null && user.IsRestricted)
            {
                user.IsRestricted = false;
                _userRepository.UpdateUser(user);
            }
        }
    }
}
