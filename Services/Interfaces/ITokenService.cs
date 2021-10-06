using Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Services
{
	public interface ITokenService
	{
		Task<LoginResult> CreateToken(User user);
		Task<ClaimsPrincipal> GetByValue(string Value);
	}
}
