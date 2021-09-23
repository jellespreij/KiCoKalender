﻿using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using KiCoKalender.Attributes;
using KiCoKalender.Models;
using KiCoKalender.Service;
using System.IO;
using KiCoKalender.Controllers;

namespace KiCoKalender.Controllers
{
    public class LoginController
    {
        ILogger Logger { get; }
        ITokenService TokenService { get; }

        public LoginController(ITokenService TokenService, ILogger<UserHttpTrigger> Logger)
        {
            this.TokenService = TokenService;
            this.Logger = Logger;
        }

        [Function(nameof(LoginController.Login))]
        [OpenApiOperation(operationId: "Login", tags: new[] { "Login" }, Summary = "Login for a user",
                            Description = "This method logs in the user, and retrieves a JWT bearer token.")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(LoginRequest), Required = true, Description = "The user credentials")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(LoginResult), Description = "Login success")]
        public async Task<HttpResponseData> Login([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req, FunctionContext executionContext)
        {
            LoginRequest login = JsonConvert.DeserializeObject<LoginRequest>(await new StreamReader(req.Body).ReadToEndAsync());

            LoginResult result = await TokenService.CreateToken(login);

            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function(nameof(LoginController.ResetPassword))]
        [OpenApiOperation(operationId: "ResetPassword", tags: new[] { "Login" }, Summary = "Reset password for a user",
                           Description = "This method sends user a reset password email.")]
        [OpenApiParameter(name: "email", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "Email of user for reset password", Description = "Email of user", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(string), Required = true, Description = "The user email")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "Reset password email send")]
        public async Task<HttpResponseData> ResetPassword([HttpTrigger(AuthorizationLevel.Anonymous, "post")]
        HttpRequestData req,
        string email,
        FunctionContext executionContext)
        {
            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

            if (string.IsNullOrEmpty(email))
            {
                response = req.CreateResponse(HttpStatusCode.BadRequest);
            }
            else
            {
                Logger.LogInformation("reset pass email send");
            }

            return response;
        }
    }
}