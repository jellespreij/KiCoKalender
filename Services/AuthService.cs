using Models;
using Repositories;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Services
{
    public class AuthService : IAuthService
    {
        private IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }
        public void RegistrateUser(User user)
        {
            User userObject = _authRepository.FindUser(e => e.Username == user.Username);
            if (userObject is null )
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                _authRepository.Add(user);
            }

        }
    }
}
