using Models;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public void AddUser(User user)
        {
            _userRepository.Add(user);
        }

        public void DeleteUser(User user)
        {
            _userRepository.Delete(user);
        }

        public User FindUserById(long userId)
        {
            return _userRepository.GetSingle(userId);
        }

        public User FindUserByName(string name)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(User user)
        {
            _userRepository.Update(user);
        }
    }
}
