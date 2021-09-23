using KiCoKalender.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KiCoKalender.Controllers
{
    class FamilyHttpTrigger
    {
		ILogger Logger { get; }

		public FamilyHttpTrigger(ILogger<FamilyHttpTrigger> Logger)
		{
			this.Logger = Logger;
		}

		[Function(nameof(FamilyHttpTrigger.FindFamilyByUserId))]
		[OpenApiOperation(operationId: "FindFamilyByUserId", tags: new[] { "family" }, Summary = "Find family by userId", Description = "Returns family by userId.", Visibility = OpenApiVisibilityType.Important)]
		//[OpenApiSecurity("petstore_auth", SecuritySchemeType.Http, In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
		[OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(long?), Summary = "userId of family to return", Description = "userId of family to return", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Asset), Summary = "successful operation", Description = "successful operation", Example = typeof(DummyFamilyExample))]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "family not found", Description = "family not found")]
		public async Task<HttpResponseData> FindFamilyByUserId(
			[HttpTrigger(AuthorizationLevel.Function,
			"GET", Route = "family/{user-id}")]
			HttpRequestData req,
			long? userId,
			FunctionContext executionContext)
		{
			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

			if (!userId.HasValue)
			{
				response = req.CreateResponse(HttpStatusCode.BadRequest);
			}
			else
			{
				Logger.LogInformation("Find family of user by userId");
			}

			return response;
		}

		[Function(nameof(FamilyHttpTrigger.AddFamily))]
		[OpenApiOperation(operationId: "AddFamily", tags: new[] { "family" }, Summary = "Add a family to the KiCoKalender", Description = "This adds a family to the KiCoKalender.", Visibility = OpenApiVisibilityType.Important)]
		//[OpenApiSecurity("petstore_auth", SecuritySchemeType.Http, In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
		[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Family), Required = true, Description = "family object that needs to be added to the KiCoKalender", Example = typeof(DummyFamilyExample))]
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

			if (family == null)
			{
				response = req.CreateResponse(HttpStatusCode.BadRequest);
			}
			else
			{
				Logger.LogInformation("Adds family");
			}

			return response;
		}

		[Function(nameof(FamilyHttpTrigger.DeleteFamily))]
		[OpenApiOperation(operationId: "DeleteFamily", tags: new[] { "family" }, Summary = "Deletes a family from the KiCoKalender", Description = "This Deletes a family from the KiCoKalender.", Visibility = OpenApiVisibilityType.Important)]
		//[OpenApiSecurity("petstore_auth", SecuritySchemeType.Http, In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
		[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Family), Required = true, Description = "family object that needs to be Deleted from the KiCoKalender", Example = typeof(DummyFamilyExample))]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(User), Summary = "Asset Deleted", Description = "Asset deleted")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Invalid input", Description = "Invalid input")]
		public async Task<HttpResponseData> DeleteFamily(
			[HttpTrigger(AuthorizationLevel.Function,
			"DELETE", Route = "family")]
			HttpRequestData req,
			FunctionContext executionContext)
		{
			// Parse input
			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			Family family = JsonConvert.DeserializeObject<Family>(requestBody);

			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

			if (family == null)
			{
				response = req.CreateResponse(HttpStatusCode.BadRequest);
			}
			else
			{
				Logger.LogInformation("Deletes family");
			}

			return response;
		}

		[Function(nameof(FamilyHttpTrigger.UpdateFamily))]
		[OpenApiOperation(operationId: "updateFamily", tags: new[] { "family" }, Summary = "Update an existing family", Description = "This updates an existing family.", Visibility = OpenApiVisibilityType.Important)]
		//[OpenApiSecurity("petstore_auth", SecuritySchemeType.Http, In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
		[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Family), Required = true, Description = "Family object that needs to be updated")]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Family), Summary = "Family details updated", Description = "Family details updated", Example = typeof(DummyFamilyExample))]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Family not found", Description = "Family not found")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
		public async Task<HttpResponseData> UpdateFamily(
			[HttpTrigger(AuthorizationLevel.Function, "PUT", Route = "family")]
			HttpRequestData req,
			FunctionContext executionContext)
		{
			// Parse input
			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			Family family = JsonConvert.DeserializeObject<Family>(requestBody);

			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

			if (family == null)
			{
				response = req.CreateResponse(HttpStatusCode.BadRequest);
			}
			else
			{
				Logger.LogInformation("Updates family");
			}

			await response.WriteAsJsonAsync(family);

			return response;
		}

	}
}
