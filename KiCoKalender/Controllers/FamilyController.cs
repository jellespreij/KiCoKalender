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
using System.Collections.Generic;
using System;
using Auth.Interfaces;
using System.Security.Claims;
using Attributes;

namespace Controllers
{
    class FamilyController
    {
		ILogger Logger { get; }
		IFamilyService FamilyService { get; }
		IAuthenticate Authenticate { get; }

		public FamilyController(ILogger<FamilyController> Logger, IFamilyService familyService, IAuthenticate authenticate)
		{
			this.Logger = Logger;
			FamilyService = familyService;
			Authenticate = authenticate;
		}

		[Function("AddFamily")]
		[UserAuth]
		[OpenApiOperation(operationId: "AddFamily", tags: new[] { "family" }, Summary = "Add a family to the KiCoKalender", Description = "This adds a family to the KiCoKalender.", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Family), Required = true, Description = "Family that needs to be added to the KiCoKalender", Example = typeof(DummyFamilyExample))]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Family), Summary = "New family added", Description = "New family added")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Invalid input", Description = "Invalid input")]
		[UnauthorizedResponse]
		[ForbiddenResponse]
		public async Task<HttpResponseData> AddFamily(
			[HttpTrigger(AuthorizationLevel.Function,
			"POST", Route = "family")]
			HttpRequestData req,
			FunctionContext executionContext)
		{
			return await Authenticate.ExecuteForUser(req, executionContext, async (ClaimsPrincipal User) => {
			// Parse input
			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			Family family = JsonConvert.DeserializeObject<Family>(requestBody);

			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

			if (family is null)
			{
				response = req.CreateResponse(HttpStatusCode.BadRequest);
			}
			else
			{
				FamilyService.AddFamily(family);
			}

			return response;
			});
		}

		[Function("FindFamilyByUserIdAndRole")]
		[UserAuth]
		[OpenApiOperation(operationId: "FindFamilyByUserIdAndRole", tags: new[] { "family" }, Summary = "Find family by userId and role", Description = "Returns a family by userId and role.", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(long?), Summary = "userId of family to return", Description = "userId of family to return", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiParameter(name: "role", In = ParameterLocation.Query, Required = true, Type = typeof(Role), Summary = "role of user of family to return", Description = "role of user of family to return", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IEnumerable<Family>), Summary = "successful operation", Description = "successful operation", Example = typeof(DummyFamilyExample))]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid Id supplied", Description = "Invalid Id supplied")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "User not found", Description = "User not found")]
		[UnauthorizedResponse]
		[ForbiddenResponse]
		public async Task<HttpResponseData> FindFamilyByUserIdAndRole(
			[HttpTrigger(AuthorizationLevel.Function, "GET", Route = "family/{userId}")]
			HttpRequestData req,
			Guid userId,
			string role,
			FunctionContext executionContext)
		{
			return await Authenticate.ExecuteForUser(req, executionContext, async (ClaimsPrincipal User) => {
			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

			if (role is null)
			{
				response = req.CreateResponse(HttpStatusCode.BadRequest);
			}
			else
			{
				await response.WriteAsJsonAsync(FamilyService.FindFamilyByUserIdAndRole(userId, (Role)Enum.Parse(typeof(Role), role)));
			}

			return response;
			});
		}

		[Function("SendMail")]
		[UserAuth]
		[OpenApiOperation(operationId: "SendMail", tags: new[] { "family" }, Summary = "Send email to other user to invite them to the KiCoKalender",
			Description = "This method sends a mail to invite a other user")]
		[OpenApiParameter(name: "email", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "Email to send the email to", Description = "Email to send to", Visibility = OpenApiVisibilityType.Important)]
		[UnauthorizedResponse]
		[ForbiddenResponse]
		public async Task<HttpResponseData> SendMail([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req, string email, FunctionContext executionContext)
		{
			return await Authenticate.ExecuteForUser(req, executionContext, async (ClaimsPrincipal User) => {
				HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
				FamilyService.SendMail(email);

				return response;
			});
		}
	}
}
