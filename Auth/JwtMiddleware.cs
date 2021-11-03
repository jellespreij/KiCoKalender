using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Services;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Auth {
    public class JwtMiddleware : IFunctionsWorkerMiddleware {
        ITokenService TokenService { get; }
        ILogger Logger { get; }

        public JwtMiddleware(ITokenService TokenService, ILogger<JwtMiddleware> Logger) {
            this.TokenService = TokenService;
            this.Logger = Logger;
        }

        public async Task Invoke(FunctionContext Context, FunctionExecutionDelegate Next) {
            string HeadersString = (string)Context.BindingContext.BindingData["Headers"];

            Dictionary<string, string> Headers = JsonConvert.DeserializeObject<Dictionary<string, string>>(HeadersString);

            if (Headers.TryGetValue("Authorization", out string AuthorizationHeader)) {
                try {
                    AuthenticationHeaderValue BearerHeader = AuthenticationHeaderValue.Parse(AuthorizationHeader);

                    ClaimsPrincipal User = await TokenService.GetByValue(BearerHeader.Parameter);

                    Context.Items["User"] = User;
                }
                catch (Exception e) {
                    Logger.LogError(e.Message);
                }
            }

            await Next(Context);
        }

    }
}


