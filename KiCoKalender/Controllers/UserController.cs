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

namespace Controllers
{
	class UserController
	{
		public ILogger Logger { get; }
		IUserService UserService { get; }

		public UserController(ILogger<UserController> Logger, IUserService userService)
		{
			this.Logger = Logger;
			UserService = userService;
		}

		[Function("FindUserById")]
		[OpenApiOperation(operationId: "FindUserById", tags: new[] { "user" }, Summary = "Find user by ID", Description = "Returns a user by ID.", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "Id of user to return", Description = "Id of user to return", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(User), Summary = "successful operation", Description = "successful operation", Example = typeof(DummyUserExample))]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid Id supplied", Description = "Invalid Id supplied")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "User not found", Description = "User not found")]
		public async Task<HttpResponseData> FindUserById(
			[HttpTrigger(AuthorizationLevel.Function, "GET", Route = "user/{userId}")]
			HttpRequestData req,
			long userId,
			FunctionContext executionContext)
		{
			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

			await response.WriteAsJsonAsync(UserService.FindUserById(userId));

			return response;
		}

		[Function("AddUser")]
		[OpenApiOperation(operationId: "AddUser", tags: new[] { "user" }, Summary = "Add a user to the KiCoKalender", Description = "This adds a new user to the KiCoKalender.", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(User), Required = true, Description = "User object that needs to be added to the KiCoKalender")]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(User), Summary = "New user added", Description = "New user added", Example = typeof(DummyUserExample))]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid input", Description = "Invalid input")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
		public async Task<HttpResponseData> AddUser(
			[HttpTrigger(AuthorizationLevel.Function, "POST", Route = "user")]
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
				UserService.AddUser(user);
			}

			await response.WriteAsJsonAsync(user);

			return response;
		}

		[Function("DeleteUser")]
		[OpenApiOperation(operationId: "deleteUser", tags: new[] { "user" }, Summary = "Deletes a user from the KiCoKalender", Description = "Deletes a user from the KiCoKalender.", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(User), Required = true, Description = "User object that needs to be Deleted from the KiCoKalender")]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(User), Summary = "New user Delete", Description = "user deleted")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid input", Description = "Invalid input")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "User not found", Description = "User not found")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
		public async Task<HttpResponseData> DeleteUser(
			[HttpTrigger(AuthorizationLevel.Function,
			"DELETE", Route = "user")]
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
				UserService.DeleteUser(user);
			}

			await response.WriteAsJsonAsync(user);

			return response;
		}

		[Function("UpdateUser")]
		[OpenApiOperation(operationId: "updateUser", tags: new[] { "user" }, Summary = "Update an existing user", Description = "This updates an existing user.", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(User), Required = true, Description = "User that needs to be updated")]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(User), Summary = "User details updated", Description = "User details updated", Example = typeof(DummyUserExample))]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "User not found", Description = "User not found")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
		public async Task<HttpResponseData> UpdateUser(
			[HttpTrigger(AuthorizationLevel.Function, "PUT", Route = "user")]
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
				UserService.UpdateUser(user);
			}

			await response.WriteAsJsonAsync(user);

			return response;
		}
	}
}