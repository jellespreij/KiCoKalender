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
    class AssetController
    {
		ILogger Logger { get; }
		IAssetService AssetService { get; }

		public AssetController(ILogger<AssetController> Logger, IAssetService assetService)
		{
			this.Logger = Logger;
			AssetService = assetService;
		}

		[Function("AddAsset")]
		[OpenApiOperation(operationId: "AddAsset", tags: new[] { "asset" }, Summary = "Add an asset to the KiCoKalender", Description = "This adds an asset to the KiCoKalender.", Visibility = OpenApiVisibilityType.Important)]
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

			if (asset is null)
			{
				response = req.CreateResponse(HttpStatusCode.BadRequest);
			}
			else
			{
				AssetService.AddAsset(asset);
			}

			return response;
		}

		[Function("FindAssetsByUserIdAndFolder")]
		[OpenApiOperation(operationId: "FindAssetsByUserIdAndFolder", tags: new[] { "asset" }, Summary = "Find assets by userId and folder", Description = "Returns assets by userId and folder.", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "userId of Assets to return", Description = "userId of Assets to return", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiParameter(name: "folder", In = ParameterLocation.Query, Required = true, Type = typeof(List<Folder>), Summary = "folder of Assets to return", Description = "folder of Assets to return", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IEnumerable<Asset>), Summary = "successful operation", Description = "successful operation", Example = typeof(DummyAssetExample))]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID or Role supplied", Description = "Invalid ID or Role supplied")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Assets not found", Description = "Assets not found")]
		public async Task<HttpResponseData> FindAssetsByUserIdAndFolder(
			[HttpTrigger(AuthorizationLevel.Function,
			"GET", Route = "asset/{userId}")]
			HttpRequestData req,
			long userId,
			string folder,
			FunctionContext executionContext)
		{
			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

			if (folder is null)
			{
				response = req.CreateResponse(HttpStatusCode.BadRequest);
			}
			else
			{
				await response.WriteAsJsonAsync(AssetService.FindAssetByUserIdAndFolder(userId, (Folder)Enum.Parse(typeof(Folder), folder)));
			}

			return response;
		}

		[Function("FindAssetsByFamilyIdAndFolder")]
		[OpenApiOperation(operationId: "FindAssetsByFamilyIdAndFolder", tags: new[] { "asset" }, Summary = "Find assets by familyId and folder", Description = "Returns assets by familyId and folder.", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiParameter(name: "familyId", In = ParameterLocation.Query, Required = true, Type = typeof(long), Summary = "familyId of Assets to return", Description = "familyId of Assets to return", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiParameter(name: "folder", In = ParameterLocation.Query, Required = true, Type = typeof(List<Folder>), Summary = "folder of Assets to return", Description = "folder of Assets to return", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IEnumerable<Asset>), Summary = "successful operation", Description = "successful operation", Example = typeof(DummyAssetExample))]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Assets not found", Description = "Assets not found")]
		public async Task<HttpResponseData> FindAssetsByFamilyIdAndFolder(
			[HttpTrigger(AuthorizationLevel.Function,
			"GET", Route = "asset")]
			HttpRequestData req,
			long familyId,
			string folder,
			FunctionContext executionContext)
		{
			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

			if (folder is null)
			{
				response = req.CreateResponse(HttpStatusCode.BadRequest);
			}
			else
			{
				await response.WriteAsJsonAsync(AssetService.FindAssetByFamilyIdAndFolder(familyId, (Folder)Enum.Parse(typeof(Folder), folder)));
			}

			return response;
		}

		[Function("DeleteAsset")]
		[OpenApiOperation(operationId: "DeleteAsset", tags: new[] { "asset" }, Summary = "Deletes an asset from the KiCoKalender", Description = "This Deletes an asset from the KiCoKalender.", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Asset), Required = true, Description = "Asset object that needs to be Deleted from the KiCoKalender", Example = typeof(DummyAssetExample))]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(UserContext), Summary = "Asset Deleted", Description = "Asset deleted")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid input", Description = "Invalid input")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Asset not found", Description = "Asset not found")]
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

			if (asset is null)
			{
				response = req.CreateResponse(HttpStatusCode.BadRequest);
			}
			else
			{
				AssetService.DeleteAsset(asset);
			}

			return response;
		}

		[Function("UpdateAsset")]
		[OpenApiOperation(operationId: "UpdateAsset", tags: new[] { "asset" }, Summary = "Update an existing Asset", Description = "This updates an existing Asset.", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Asset), Required = true, Description = "Asset object that needs to be updated")]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Asset), Summary = "Asset details updated", Description = "Asset details updated", Example = typeof(DummyAssetExample))]
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

			if (asset is null)
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
	}
}
