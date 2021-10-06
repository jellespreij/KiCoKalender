using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Models;
using Newtonsoft.Json;
using Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Controllers
{
    public class AuthController
    {
		public ILogger Logger { get; }
		ITokenService TokenService { get; }

		IAuthService AuthService { get; }

		public AuthController(ILogger<AuthController> Logger, ITokenService tokenService, IAuthService authService)
		{
			this.Logger = Logger;
			TokenService = tokenService;
			AuthService = authService;
		}

        [Function(nameof(AuthController.Login))]
        [OpenApiOperation(operationId: "Login", tags: new[] { "auth" }, Summary = "Login for a user",
                    Description = "This method logs in the user, and retrieves a JWT bearer token.")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(User), Required = true, Description = "The user credentials")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(LoginResult), Description = "Login success")]
        public async Task<HttpResponseData> Login([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req, FunctionContext executionContext)
        {
            User login = JsonConvert.DeserializeObject<User>(await new StreamReader(req.Body).ReadToEndAsync());

            LoginResult result = await TokenService.CreateToken(login);

            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }

		[Function(nameof(AuthController.Registration))]
		[OpenApiOperation(operationId: "Registration", tags: new[] { "auth" }, Summary = "Registrate a user to the KiCoKalender", Description = "This Registrates a new user to the KiCoKalender.", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(User), Required = true, Description = "User-context that needs to be added to the KiCoKalender")]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(User), Summary = "New user registrated", Description = "New user registrated")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid input", Description = "Invalid input")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
		public async Task<HttpResponseData> Registration(
		[HttpTrigger(AuthorizationLevel.Function, "POST")]
			HttpRequestData req,
			FunctionContext executionContext)
		{
			// Parse input
			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			User user = JsonConvert.DeserializeObject<User>(requestBody);

			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

			if (user is null)
			{
				response = req.CreateResponse(HttpStatusCode.BadRequest);
			}
			else
			{
				AuthService.RegistrateUser(user);
			}

			await response.WriteAsJsonAsync(user);

			return response;
		}
	}
}
