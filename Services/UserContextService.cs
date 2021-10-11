using Models;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UserContextService : IUserContextService
    {
        private IUserContextRepository _userContextRepository;

        public UserContextService(IUserContextRepository userContextRepository)
        {
            _userContextRepository = userContextRepository;
        }

        public void AddUserContext(UserContext userContext)
        {
            _userContextRepository.Add(userContext);
        }

        public void DeleteUserContext(UserContext userContext)
        {
            _userContextRepository.Delete(userContext);
        }

        public UserContext FindUserContextByUserId(Guid userId)
        {
            return _userContextRepository.GetSingle(userId);
        }

        public UserContext FindUserByName(string name)
        {
            throw new NotImplementedException();
        }

        public void UpdateUserContext(UserContext userContext)
        {
            _userContextRepository.Update(userContext);
        }
    }
}
