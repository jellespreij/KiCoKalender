using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using KiCoKalender.Models;
using Microsoft.OpenApi.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.IO;
using Newtonsoft.Json;
using KiCoKalender.Service;
using KiCoKalender.Interfaces;

namespace KiCoKalender.Controllers
{
    class AssetsHttpTrigger
    {
		ILogger Logger { get; }
		IUserService UserService { get; }

		public AssetsHttpTrigger(ILogger<AssetsHttpTrigger> Logger, IUserService userService)
		{
			this.Logger = Logger;
			this.UserService = userService;
		}

		[Function(nameof(AssetsHttpTrigger.GetAssetById))]
		[OpenApiOperation(operationId: "GetAssetById", tags: new[] { "asset" }, Summary = "Find asset by ID", Description = "Returns a Asset.", Visibility = OpenApiVisibilityType.Important)]
		//[OpenApiSecurity("petstore_auth", SecuritySchemeType.Http, In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
		[OpenApiParameter(name: "assetId", In = ParameterLocation.Path, Required = true, Type = typeof(long?), Summary = "ID of Asset to return", Description = "ID of Asset to return", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Asset), Summary = "successful operation", Description = "successful operation")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Asset not found", Description = "Asset not found")]
		public async Task<HttpResponseData> GetAssetById(
			[HttpTrigger(AuthorizationLevel.Function,
			"GET", Route = "asset/{assetId}")]
			HttpRequestData req,
			long? assetId,
			FunctionContext executionContext)
		{
			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

			if (assetId.HasValue)
			{
				response = req.CreateResponse(HttpStatusCode.BadRequest);
			}
			else
			{
				//await response.WriteAsJsonAsync(AssetsService.GetAssetsById(assetId));
			}

			return response;
		}

		[Function(nameof(AssetsHttpTrigger.AddAsset))]
		[OpenApiOperation(operationId: "addAsset", tags: new[] { "asset" }, Summary = "Add a asset to the KiCoKalender", Description = "This adds a asset to the KiCoKalender.", Visibility = OpenApiVisibilityType.Important)]
		//[OpenApiSecurity("petstore_auth", SecuritySchemeType.Http, In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
		[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Asset), Required = true, Description = "Asset object that needs to be added to the KiCoKalender")]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Asset), Summary = "New asset added", Description = "New asset added")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Invalid input", Description = "Invalid input")]
		public async Task<HttpResponseData> AddAsset(
			[HttpTrigger(AuthorizationLevel.Function,
			"POST", Route = "asset")]
			HttpRequestData req,
			FunctionContext executionContext)
		{
			// Parse input
			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			Asset asset = JsonConvert.DeserializeObject<Asset>(requestBody);

			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

			if (asset == null)
			{
				response = req.CreateResponse(HttpStatusCode.BadRequest);
			}
			else
			{
				//UserService.AddAsset(asset);
			}

			return response;
		}

		[Function(nameof(AssetsHttpTrigger.DeleteAsset))]
		[OpenApiOperation(operationId: "deleteAsset", tags: new[] { "asset" }, Summary = "Deletes a asset to the KiCoKalender", Description = "This Deletes an asset to the KiCoKalender.", Visibility = OpenApiVisibilityType.Important)]
		//[OpenApiSecurity("petstore_auth", SecuritySchemeType.Http, In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
		[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Asset), Required = true, Description = "Asset object that needs to be Deleted from the KiCoKalender")]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(User), Summary = "New asset Delete", Description = "Asset deleted")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Invalid input", Description = "Invalid input")]
		public async Task<HttpResponseData> DeleteAsset(
			[HttpTrigger(AuthorizationLevel.Function,
			"DELETE", Route = "asset")]
			HttpRequestData req,
			FunctionContext executionContext)
		{
			// Parse input
			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			Asset asset = JsonConvert.DeserializeObject<Asset>(requestBody);

			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

			if (asset == null)
			{
				response = req.CreateResponse(HttpStatusCode.BadRequest);
			}
			else
			{
				//UserService.AddAsset(asset);
			}

			return response;
		}

		[Function(nameof(AssetsHttpTrigger.UpdateAsset))]
		[OpenApiOperation(operationId: "updateAsset", tags: new[] { "asset" }, Summary = "Update an existing Asset", Description = "This updates an existing Asset.", Visibility = OpenApiVisibilityType.Important)]
		//[OpenApiSecurity("petstore_auth", SecuritySchemeType.Http, In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
		[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Asset), Required = true, Description = "Asset object that needs to be updated")]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Asset), Summary = "Asset details updated", Description = "Asset details updated")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Asset not found", Description = "Asset not found")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
		public async Task<HttpResponseData> UpdateAsset(
			[HttpTrigger(AuthorizationLevel.Function, "PUT", Route = "asset")]
			HttpRequestData req,
			FunctionContext executionContext)
		{
			// Parse input
			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			Asset asset = JsonConvert.DeserializeObject<Asset>(requestBody);

			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

			await response.WriteAsJsonAsync(asset);

			return response;
		}

		[Function(nameof(AssetsHttpTrigger.GetAssetByAssetEnum))]
		[OpenApiOperation(operationId: "GetAssetByAssetEnum", tags: new[] { "asset" }, Summary = "Find asset by ID", Description = "Returns a Asset.", Visibility = OpenApiVisibilityType.Important)]
		//[OpenApiSecurity("petstore_auth", SecuritySchemeType.Http, In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
		[OpenApiParameter(name: "assetsEnum", In = ParameterLocation.Path, Required = true, Type = typeof(List<AssetsEnum>), Summary = "AssetEnum of Asset to return", Description = "AssetEnum of Asset to return", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Asset), Summary = "successful operation", Description = "successful operation")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid assetEnum supplied", Description = "Invalid AssetEnum supplied")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Asset not found", Description = "Asset not found")]
		public async Task<HttpResponseData> GetAssetByAssetEnum(
			[HttpTrigger(AuthorizationLevel.Function,
			"GET", Route = "asset/GetAssetByAssetEnum")]
			HttpRequestData req,
			AssetsEnum assetsEnum,
			FunctionContext executionContext)
		{
			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

			if (!System.Enum.IsDefined(typeof(AssetsEnum), assetsEnum))
			{
				response = req.CreateResponse(HttpStatusCode.BadRequest);
			}
			else
			{
				//await response.WriteAsJsonAsync(AssetsService.GetAssetsById(assetId));
			}

			return response;
		}
	}
}
