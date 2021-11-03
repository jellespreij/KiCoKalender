using Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Services.Interfaces
{
	public interface ITokenService
	{
		Task<LoginResult> CreateToken(Login login);
		Task<ClaimsPrincipal> GetByValue(string Value);
	}
}
