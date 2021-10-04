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

namespace Controllers
{
    class FamilyController
    {
		ILogger Logger { get; }
		IFamilyService FamilyService { get; }

		public FamilyController(ILogger<FamilyController> Logger, IFamilyService familyService)
		{
			this.Logger = Logger;
			FamilyService = familyService;
		}

		[Function("AddFamily")]
		[OpenApiOperation(operationId: "AddFamily", tags: new[] { "family" }, Summary = "Add a family to the KiCoKalender", Description = "This adds a family to the KiCoKalender.", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Family), Required = true, Description = "Family that needs to be added to the KiCoKalender", Example = typeof(DummyFamilyExample))]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Family), Summary = "New family added", Description = "New family added")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Invalid input", Description = "Invalid input")]
		public async Task<HttpResponseData> AddFamily(
			[HttpTrigger(AuthorizationLevel.Function,
			"POST", Route = "family")]
			HttpRequestData req,
			FunctionContext executionContext)
		{
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
		}

		[Function("FindFamilyByUserIdAndRole")]
		[OpenApiOperation(operationId: "FindFamilyByUserIdAndRole", tags: new[] { "family" }, Summary = "Find family by userId and role", Description = "Returns a family by userId and role.", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(long?), Summary = "userId of family to return", Description = "userId of family to return", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiParameter(name: "role", In = ParameterLocation.Query, Required = true, Type = typeof(Role), Summary = "role of user of family to return", Description = "role of user of family to return", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IEnumerable<Family>), Summary = "successful operation", Description = "successful operation", Example = typeof(DummyFamilyExample))]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid Id supplied", Description = "Invalid Id supplied")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "User not found", Description = "User not found")]
		public async Task<HttpResponseData> FindFamilyByUserIdAndRole(
			[HttpTrigger(AuthorizationLevel.Function, "GET", Route = "family/{userId}")]
			HttpRequestData req,
			long userId,
			string role,
			FunctionContext executionContext)
		{
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
		}
	}
}
