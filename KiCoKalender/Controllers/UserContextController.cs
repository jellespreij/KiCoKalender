using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;
using System.Net;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.Functions.Worker.Http;
using Models;
using Services;
using System.IO;
using Newtonsoft.Json;
using Attributes;
using System.Security.Claims;
using Auth;
using Auth.Interfaces;
using System;

namespace Controllers
{
	class UserContextController
	{
		public ILogger Logger { get; }
		IUserContextService UserService { get; }
		IAuthenticate Authenticate { get; }

		public UserContextController(ILogger<UserContextController> Logger, IUserContextService userService, IAuthenticate authenticate)
		{
			this.Logger = Logger;
			UserService = userService;
			Authenticate = authenticate;
		}

		[Function("FindUserContextByUserId")]
		[UserAuth]
		[OpenApiOperation(operationId: "FindUserContextByUserId", tags: new[] { "user-context" }, Summary = "Find user-context by ID", Description = "Returns the user-context by ID.", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Summary = "UserId of user-context to return", Description = "UserId of user-context to return", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(UserContext), Summary = "successful operation", Description = "successful operation", Example = typeof(DummyUserContextExample))]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid Id supplied", Description = "Invalid Id supplied")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "User not found", Description = "User not found")]
		[UnauthorizedResponse]
		[ForbiddenResponse]
		public async Task<HttpResponseData> FindUserContextByUserId(
			[HttpTrigger(AuthorizationLevel.Function, "GET", Route = "user-context/{userId}")]
			HttpRequestData req,
			String userId,
			FunctionContext executionContext)
		{
			return await Authenticate.ExecuteForUser(req, executionContext, async (ClaimsPrincipal User) => {
				// Generate output
				HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

				await response.WriteAsJsonAsync(UserService.FindUserContextByUserId(Guid.Parse(userId)));

				return response;
			});

		}

		[Function("AddUserContext")]
		[UserAuth]
		[OpenApiOperation(operationId: "AddUserContext", tags: new[] { "user-context" }, Summary = "Add a user-context to the KiCoKalender", Description = "This adds a new user-context to the KiCoKalender.", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(UserContext), Required = true, Description = "User-context that needs to be added to the KiCoKalender")]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(UserContext), Summary = "New user-context added", Description = "New user-context added", Example = typeof(DummyUserContextExample))]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid input", Description = "Invalid input")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
		[UnauthorizedResponse]
		[ForbiddenResponse]
		public async Task<HttpResponseData> AddUser(
			[HttpTrigger(AuthorizationLevel.Function, "POST", Route = "user-context")]
			HttpRequestData req,
			FunctionContext executionContext)
		{
			return await Authenticate.ExecuteForUser(req, executionContext, async (ClaimsPrincipal User) => {
				// Parse input
				string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			UserContext user = JsonConvert.DeserializeObject<UserContext>(requestBody);

			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

			if (user is null)
			{
				response = req.CreateResponse(HttpStatusCode.BadRequest);
			}
			else
			{
				UserService.AddUserContext(user);
			}

			await response.WriteAsJsonAsync(user);

			return response;
			});
		}

		[Function("DeleteUserContext")]
		[UserAuth]
		[OpenApiOperation(operationId: "deleteUserContext", tags: new[] { "user-context" }, Summary = "Deletes a user-context from the KiCoKalender", Description = "Deletes a user-context from the KiCoKalender.", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(UserContext), Required = true, Description = "User-context that needs to be Deleted from the KiCoKalender")]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(UserContext), Summary = "New user-context Delete", Description = "user-context deleted", Example = typeof(DummyUserContextExample))]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid input", Description = "Invalid input")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "User not found", Description = "User not found")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
		[UnauthorizedResponse]
		[ForbiddenResponse]
		public async Task<HttpResponseData> DeleteUserContext(
			[HttpTrigger(AuthorizationLevel.Function,
			"DELETE", Route = "user-context")]
			HttpRequestData req,
			FunctionContext executionContext)
		{
			return await Authenticate.ExecuteForUser(req, executionContext, async (ClaimsPrincipal User) => {
				// Parse input
				string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			UserContext userContext = JsonConvert.DeserializeObject<UserContext>(requestBody);

			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

			if (userContext is null)
			{
				response = req.CreateResponse(HttpStatusCode.BadRequest);
			}
			else
			{
				UserService.DeleteUserContext(userContext);
			}

			await response.WriteAsJsonAsync(userContext);

			return response;
		});
		}

		[Function("UpdateUserContext")]
		[UserAuth]
		[OpenApiOperation(operationId: "updateUserContext", tags: new[] { "user-context" }, Summary = "Update an existing user-context", Description = "This updates an existing user-context.", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(UserContext), Required = true, Description = "User-context that needs to be updated")]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(UserContext), Summary = "User-context details updated", Description = "User-context details updated", Example = typeof(DummyUserContextExample))]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "User-context not found", Description = "User-context not found")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
		[UnauthorizedResponse]
		[ForbiddenResponse]
		public async Task<HttpResponseData> UpdateUser(
			[HttpTrigger(AuthorizationLevel.Function, "PUT", Route = "user-context")]
			HttpRequestData req,
			FunctionContext executionContext)
		{
			return await Authenticate.ExecuteForUser(req, executionContext, async (ClaimsPrincipal User) => {
			// Parse input
			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			UserContext userContext = JsonConvert.DeserializeObject<UserContext>(requestBody);

			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

			if (userContext is null)
			{
				response = req.CreateResponse(HttpStatusCode.BadRequest);
			}
			else
			{
				UserService.UpdateUserContext(userContext);
			}

			await response.WriteAsJsonAsync(userContext);

			return response;
		});
		}
	}
}