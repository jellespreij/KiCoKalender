﻿using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;
using System.Net;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.Functions.Worker.Http;
using Models;
using System.Linq;
using Services;
using System.IO;
using Newtonsoft.Json;
using Auth.Attributes;
using System.Security.Claims;
using Auth;
using Auth.Interfaces;
using System;
using System.Collections.Generic;
using Services.Interfaces;
using HttpMultipartParser;

namespace KiCoKalender.Controllers
{
    public class UserController
    {
        public ILogger Logger { get; }
        IUserService UserService { get; }
        IValidationService ValidationService { get; }
        IAuthenticate Authenticate { get; }

        public UserController(ILogger<UserController> Logger, IUserService userService, IAuthenticate authenticate, IValidationService validationService)
        {
            this.Logger = Logger;
            UserService = userService;
            Authenticate = authenticate;
            ValidationService = validationService;
        }

        [Function("FindUserByUserId")]
        [UserAuth]
        [OpenApiOperation(operationId: "FindUserByUserId", tags: new[] { "user" }, Summary = "Find user by ID", Description = "Returns the user by ID.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Summary = "UserId of user to return", Description = "UserId of user to return", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(User), Summary = "successful operation", Description = "successful operation", Example = typeof(DummyUserExample))]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid Id supplied", Description = "Invalid Id supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "User not found", Description = "User not found")]
        [UnauthorizedResponse]
        [ForbiddenResponse]
        public async Task<HttpResponseData> FindUserByUserId(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "user/{userId}")]
            HttpRequestData req,
            String userId,
            FunctionContext executionContext)
        {
            return await Authenticate.ExecuteForUser(req, executionContext, async (ClaimsPrincipal User) =>
            {
                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

                UserDTO user = UserService.FindUserDTOByUserId(Guid.Parse(userId));

                if (user is not null)
                {
                    await response.WriteAsJsonAsync(user);
                }
                else
                {
                    response = req.CreateResponse(HttpStatusCode.NotFound);
                }

                return response;
            });

        }



        [Function("FindUserByEmail")]
        [UserAuth]
        [OpenApiOperation(operationId: "FindUserByEmail", tags: new[] { "user" }, Summary = "Find user by email", Description = "Returns a user by email.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "email", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "email of user to find", Description = "email of user to find", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(User), Summary = "successful operation", Description = "successful operation", Example = typeof(DummyUserExample))]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid email supplied", Description = "Invalid email supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "User not found", Description = "User not found")]
        [UnauthorizedResponse]
        [ForbiddenResponse]
        public async Task<HttpResponseData> FindUserByEmail(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "user/FindUser")]
            HttpRequestData req,
            string email,
            FunctionContext executionContext)
        {
            return await Authenticate.ExecuteForUser(req, executionContext, async (ClaimsPrincipal User) =>
            {

                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

                UserDTO user = UserService.FindUserDTOByEmail(email);

                if (user is null)
                {
                    response = req.CreateResponse(HttpStatusCode.NotFound);
                }
                else
                {
                    await response.WriteAsJsonAsync(user);
                }

                return response;
            });
        }

        [Function("addUser")]
        [OpenApiOperation(operationId: "addUser", tags: new[] { "user" }, Summary = "adds a user to the KiCoKalender", Description = "This adds a new user to the KiCoKalender.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(User), Required = true, Description = "User that needs to be added to the KiCoKalender", Example = typeof(DummyUserExample))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(User), Summary = "New user registrated", Description = "New user registrated")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid input", Description = "Invalid input")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Conflict, Summary = "User already exists", Description = "User already exists")]
        public async Task<HttpResponseData> addUser(
        [HttpTrigger(AuthorizationLevel.Anonymous, "POST",
            Route = "user")]
            HttpRequestData req,
            FunctionContext executionContext)
        {
            // Parse input
            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                string checkedUserInput = ValidationService.CheckUserInput(requestBody);
                User user = JsonConvert.DeserializeObject<User>(checkedUserInput);

                new System.Net.Mail.MailAddress(user.Email);

                if (!ValidationService.CheckPasswordStrenght(user.Password))
                {
                    response = req.CreateResponse(HttpStatusCode.BadRequest);
                    Logger.LogError("Password is not strong enough");
                    return response;
                }

                User addedUser = UserService.AddUser(user);

                if (addedUser is null)
                {
                    response = req.CreateResponse(HttpStatusCode.Conflict);
                }
                else
                {
                    response = req.CreateResponse(HttpStatusCode.OK);
                    await response.WriteAsJsonAsync(addedUser);
                }

            }
            catch (Exception ex)
            {
                Logger.LogError("Invalid input", ex);
                response = req.CreateResponse(HttpStatusCode.BadRequest);
            }

            return response;
        }

        [Function("DeleteUser")]
        [UserAuth]
        [OpenApiOperation(operationId: "deleteUser", tags: new[] { "user" }, Summary = "Deletes a user from the KiCoKalender", Description = "Deletes a user from the KiCoKalender.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "id", In = ParameterLocation.Query, Required = true, Type = typeof(Guid), Summary = "Id of user to return", Description = "Id of user to return", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(User), Summary = "New user Delete", Description = "user deleted", Example = typeof(DummyUserExample))]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid input", Description = "Invalid input")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "User not found", Description = "User not found")]
        [UnauthorizedResponse]
        [ForbiddenResponse]
        public async Task<HttpResponseData> DeleteUser(
            [HttpTrigger(AuthorizationLevel.Anonymous,
            "DELETE", Route = "user")]
            HttpRequestData req,
            string id,
            FunctionContext executionContext)
        {
            return await Authenticate.ExecuteForUser(req, executionContext, async (ClaimsPrincipal currentUser) =>
            {
                // Generate output
                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

                User deletedUser = UserService.DeleteUser(Guid.Parse(id), Guid.Parse(currentUser.FindFirst(ClaimTypes.Sid).Value)).Result;

                if (deletedUser is null)
                {
                    response = req.CreateResponse(HttpStatusCode.NotFound);
                }

                return response;
            });
        }

        [Function("UpdateUser")]
        [UserAuth]
        [OpenApiOperation(operationId: "updateUser", tags: new[] { "user" }, Summary = "Update an existing user", Description = "This updates an existing user.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "id", In = ParameterLocation.Query, Required = true, Type = typeof(Guid), Summary = "Id of user to return", Description = "Id of user to return", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "multipart/form-data", bodyType: typeof(UserUpdateDTO), Description = "Parameters", Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(User), Summary = "User details updated", Description = "User details updated", Example = typeof(DummyUserExample))]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "User not found", Description = "User not found")]
        [UnauthorizedResponse]
        [ForbiddenResponse]
        public async Task<HttpResponseData> UpdateUser(
            [HttpTrigger(AuthorizationLevel.Anonymous, "PUT", Route = "user")]
            HttpRequestData req,
            string id,
            FunctionContext executionContext)
        {
            return await Authenticate.ExecuteForUser(req, executionContext, async (ClaimsPrincipal currentUser) =>
            {
                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
                try
                {
                    var parsedFormBody = await MultipartFormDataParser.ParseAsync(req.Body);
                    var parameters = parsedFormBody.Parameters;

                    UserUpdateDTO userUpdate = new()
                    {
                        Email   = parameters.FirstOrDefault(x => x.Name == "email").Data,
                        Address = parameters.FirstOrDefault(x => x.Name == "address").Data,
                        PhoneNumber = parameters.FirstOrDefault(x => x.Name == "phoneNumber").Data,
                        Zipcode = parameters.FirstOrDefault(x => x.Name == "zipcode").Data,
                    };

                    User updatedUser = UserService.UpdateUser(userUpdate, Guid.Parse(id), Guid.Parse(currentUser.FindFirst(ClaimTypes.Sid).Value));

                    if (updatedUser is null)
                    {
                        response = req.CreateResponse(HttpStatusCode.NotFound);
                    }

                }
                catch(Exception ex)
                {
                    Logger.LogError("Invalid input", ex);
                    response = req.CreateResponse(HttpStatusCode.BadRequest);
                }

                return response;
            });
        }
    }
}