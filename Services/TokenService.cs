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
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Repositories.Interfaces;

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

        public async Task<LoginResult> CreateToken(Login login)
        {
            User userObject = await _authRepository.FindUser(e => e.Email == login.Email);
            if (userObject is null)
            {
                return null;
            }

            if (userObject.LoggingAttempts <= 3 || (userObject.LoggingAttempts > 3 && userObject.LastLoginTry.AddMinutes(5) <= DateTime.Now))
            {
                if (BCryptNet.Verify(login.Password, userObject.Password))
                {

                    userObject.LoggingAttempts = 0;
                    userObject.LastLoginTry = DateTime.Now;
                    await _authRepository.Update(userObject, userObject.Id);

                    JwtSecurityToken Token = await CreateToken(new Claim[] {
                        new Claim(ClaimTypes.Role, "User"),
                        new Claim(ClaimTypes.Sid, userObject.Id.ToString()),
                        new Claim(ClaimTypes.GroupSid, userObject.FamilyId.ToString())
                    });

                    return new LoginResult(Token);
                }
                else
                {
                    userObject.LoggingAttempts++;
                    userObject.LastLoginTry = DateTime.Now;
                    await _authRepository.Update(userObject, userObject.Id);
                    return null;
                }
            }
            else 
            {
                return null;
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
                Logger.LogError("Error: ", e);
                throw;
            }
        }
    }
}
