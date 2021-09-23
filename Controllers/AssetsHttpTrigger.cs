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
using KiCoKalender.Interfaces;

namespace KiCoKalender.Controllers
{
    class AssetsHttpTrigger
    {
		ILogger Logger { get; }
		IAssetService AssetService { get; }

		public AssetsHttpTrigger(ILogger<AssetsHttpTrigger> Logger, IAssetService AssetService)
		{
			this.Logger = Logger;
			this.AssetService = AssetService;
		}

		[Function(nameof(AssetsHttpTrigger.FindAssetsByUserID))]
		[OpenApiOperation(operationId: "FindAssetsByUserID", tags: new[] { "asset" }, Summary = "Find asset by userId", Description = "Returns assets by userId.", Visibility = OpenApiVisibilityType.Important)]
		//[OpenApiSecurity("petstore_auth", SecuritySchemeType.Http, In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
		[OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(long?), Summary = "userId of Assets to return", Description = "userId of Assets to return", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Asset), Summary = "successful operation", Description = "successful operation", Example = typeof(DummyAssetExample))]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Assets not found", Description = "Assets not found")]
		public async Task<HttpResponseData> FindAssetsByUserID(
			[HttpTrigger(AuthorizationLevel.Function,
			"GET", Route = "asset/{user-id}")]
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
				await response.WriteAsJsonAsync(AssetService.FindAssetByUserId(userId));
			}

			return response;
		}

		[Function(nameof(AssetsHttpTrigger.AddAsset))]
		[OpenApiOperation(operationId: "addAsset", tags: new[] { "asset" }, Summary = "Add an asset to the KiCoKalender", Description = "This adds an asset to the KiCoKalender.", Visibility = OpenApiVisibilityType.Important)]
		//[OpenApiSecurity("petstore_auth", SecuritySchemeType.Http, In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
		[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Asset), Required = true, Description = "Asset object that needs to be added to the KiCoKalender", Example = typeof(DummyAssetExample))]
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
				AssetService.AddAsset(asset);
			}

			return response;
		}

		[Function(nameof(AssetsHttpTrigger.DeleteAsset))]
		[OpenApiOperation(operationId: "deleteAsset", tags: new[] { "asset" }, Summary = "Deletes an asset from the KiCoKalender", Description = "This Deletes an asset from the KiCoKalender.", Visibility = OpenApiVisibilityType.Important)]
		//[OpenApiSecurity("petstore_auth", SecuritySchemeType.Http, In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
		[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Asset), Required = true, Description = "Asset object that needs to be Deleted from the KiCoKalender", Example = typeof(DummyAssetExample))]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(User), Summary = "Asset Deleted", Description = "Asset deleted")]
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
				AssetService.DeleteAsset(asset);
			}

			return response;
		}

		[Function(nameof(AssetsHttpTrigger.UpdateAsset))]
		[OpenApiOperation(operationId: "updateAsset", tags: new[] { "asset" }, Summary = "Update an existing Asset", Description = "This updates an existing Asset.", Visibility = OpenApiVisibilityType.Important)]
		//[OpenApiSecurity("petstore_auth", SecuritySchemeType.Http, In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
		[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Asset), Required = true, Description = "Asset object that needs to be updated")]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Asset), Summary = "Asset details updated", Description = "Asset details updated", Example = typeof(DummyAssetExample))]
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

			if (asset == null)
			{
				response = req.CreateResponse(HttpStatusCode.BadRequest);
			}
			else
			{
				AssetService.UpdateAsset(asset);
			}

			await response.WriteAsJsonAsync(asset);

			return response;
		}

		[Function(nameof(AssetsHttpTrigger.FindByAssetEnum))]
		[OpenApiOperation(operationId: "FindByAssetEnum", tags: new[] { "asset" }, Summary = "Find assets of user by assetEnum", Description = "Returns assets of user by assetEnum.", Visibility = OpenApiVisibilityType.Important)]
		//[OpenApiSecurity("petstore_auth", SecuritySchemeType.Http, In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
		[OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(long?), Summary = "userId of Asset to return", Description = "userId of Asset to return", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiParameter(name: "assetsEnum", In = ParameterLocation.Query, Required = true, Type = typeof(List<AssetsEnum>), Summary = "AssetEnum of Asset to return", Description = "AssetEnum of Asset to return", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Asset), Summary = "successful operation", Description = "successful operation", Example = typeof(DummyAssetExample))]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid assetEnum supplied", Description = "Invalid AssetEnum supplied")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Asset not found", Description = "Asset not found")]
		public async Task<HttpResponseData> FindByAssetEnum(
			[HttpTrigger(AuthorizationLevel.Function,
			"GET", Route = "asset/{user-id}/findByAssetEnum")]
			HttpRequestData req,
			long? userId,
			AssetsEnum assetsEnum,
			FunctionContext executionContext)
		{
			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

			if (!System.Enum.IsDefined(typeof(AssetsEnum), assetsEnum) || userId.HasValue)
			{
				response = req.CreateResponse(HttpStatusCode.BadRequest);
			}
			else
			{
				await response.WriteAsJsonAsync(AssetService.FindByAssetsEnum(userId, assetsEnum));
			}

			return response;
		}
	}
}
