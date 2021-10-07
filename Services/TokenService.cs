using System.Threading.Tasks;
using System.Security.Claims;
using System;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Models;
using Microsoft.Extensions.Logging;
using Repositories;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Services
{
    public class TokenService : ITokenService
    {
        private IAuthRepository _authRepository;

        public class TokenIdentityValidationParameters : TokenValidationParameters
        {
            public TokenIdentityValidationParameters(string Issuer, string Audience, SymmetricSecurityKey SecurityKey)
            {
                RequireSignedTokens = true;
                ValidAudience = Audience;
                ValidateAudience = true;
                ValidIssuer = Issuer;
                ValidateIssuer = true;
                ValidateIssuerSigningKey = true;
                ValidateLifetime = true;
                IssuerSigningKey = SecurityKey;
                AuthenticationType = JwtBearerDefaults.AuthenticationScheme;
            }
        }

        private ILogger Logger { get; }

        private string Issuer { get; }
        private string Audience { get; }
        private TimeSpan ValidityDuration { get; }

        private SigningCredentials Credentials { get; }
        private TokenIdentityValidationParameters ValidationParameters { get; }

        public TokenService(IConfiguration Configuration, ILogger<TokenService> Logger, IAuthRepository authRepository)
        {
            this.Logger = Logger;

            Issuer = "DebugIssuer";// Configuration.GetClassValueChecked("JWT:Issuer", "DebugIssuer", Logger);
            Audience = "DebugAudience";// Configuration.GetClassValueChecked("JWT:Audience", "DebugAudience", Logger);
            ValidityDuration = TimeSpan.FromDays(1);// Todo: configure
            string Key = "DebugKey DebugKey";//Configuration.GetClassValueChecked("JWT:Key", "DebugKey DebugKey", Logger);

            SymmetricSecurityKey SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));

            Credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256Signature);

            ValidationParameters = new TokenIdentityValidationParameters(Issuer, Audience, SecurityKey);
            _authRepository = authRepository;
        }

        public async Task<LoginResult> CreateToken(User user)
        {
            User userObject = _authRepository.FindUser(e => e.Username == user.Username);
            if (userObject is null || !BCryptNet.Verify(user.Password, userObject.Password))
            {
                throw new Exception("Invalid username or password");
            }
            else
            {
                JwtSecurityToken Token = await CreateToken(new Claim[] {
                new Claim(ClaimTypes.Role, "User"),
                new Claim(ClaimTypes.Name, user.Username)
                });

                return new LoginResult(Token);
            }


        }
        private async Task<JwtSecurityToken> CreateToken(Claim[] Claims)
        {
            JwtHeader Header = new JwtHeader(Credentials);

            JwtPayload Payload = new JwtPayload(Issuer,
                                                Audience,
                                                Claims,
                                                DateTime.UtcNow,
                                                DateTime.UtcNow.Add(ValidityDuration),
                                                DateTime.UtcNow);

            JwtSecurityToken SecurityToken = new JwtSecurityToken(Header, Payload);

            return await Task.FromResult(SecurityToken);
        }

        public async Task<ClaimsPrincipal> GetByValue(string Value)
        {
            if (Value == null)
            {
                throw new Exception("No Token supplied");
            }

            JwtSecurityTokenHandler Handler = new JwtSecurityTokenHandler();

            try
            {
                SecurityToken ValidatedToken;
                ClaimsPrincipal Principal = Handler.ValidateToken(Value, ValidationParameters, out ValidatedToken);

                return await Task.FromResult(Principal);
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
