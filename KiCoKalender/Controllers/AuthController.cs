using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Models;
using Newtonsoft.Json;
using Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Controllers
{
	public class AuthController
    {
        public ILogger Logger { get; }
        ITokenService TokenService { get; }
        public AuthController(ILogger<AuthController> Logger, ITokenService tokenService)
        {
            this.Logger = Logger;
            TokenService = tokenService;
        }

        [Function("login")]
        [OpenApiOperation(operationId: "login", tags: new[] { "auth" }, Summary = "Login for a user",
                    Description = "This method logs in the user, and retrieves a JWT bearer token.")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Login), Required = true, Description = "The user credentials", Example = typeof(DummyLoginExample))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(LoginResult), Description = "Login success")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "User not found", Description = "User not found")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid input", Description = "Invalid input")]

        public async Task<HttpResponseData> Login([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req, FunctionContext executionContext)
        {
            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

            try
            {
                Login login = JsonConvert.DeserializeObject<Login>(await new StreamReader(req.Body).ReadToEndAsync());
                LoginResult result = await TokenService.CreateToken(login);

                if (result is null)
                {
                    response = req.CreateResponse(HttpStatusCode.NotFound);
                }
                else
                {
                    response = req.CreateResponse(HttpStatusCode.OK);
                    await response.WriteAsJsonAsync(result);
                }
            }
            catch (Exception ex) 
            {
                Logger.LogError("Invalid input", ex);
                response = req.CreateResponse(HttpStatusCode.BadRequest);
            }

            return response;
        }
    }
}
