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
using Microsoft.AspNetCore.Http;
using Auth.Interfaces;
using System.Security.Claims;
using Auth.Attributes;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using static System.Net.Mime.MediaTypeNames;
using Newtonsoft.Json.Linq;
using System.Linq;
using HttpMultipartParser;
using Services.Interfaces;

namespace KiCoKalender.Controllers
{
    public class AssetController
    {
        ILogger Logger { get; }
        IAssetService AssetService { get; }
        IAuthenticate Authenticate { get; }
        public AssetController(ILogger<AssetController> Logger, IAssetService assetService, IAuthenticate authenticate)
        {
            this.Logger = Logger;
            AssetService = assetService;
            Authenticate = authenticate;
        }

        [Function("AddAsset")]
        [UserAuth]
        [OpenApiOperation(operationId: "AddAsset", tags: new[] { "asset" }, Summary = "Add an asset to the KiCoKalender", Description = "This adds an asset to the KiCoKalender.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "folderId", In = ParameterLocation.Query, Required = true, Type = typeof(Guid), Summary = "FolderId of Assets to return", Description = "FolderId of Assets to return", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "multipart/form-data", bodyType: typeof(IFormFile), Required = true, Description = "Asset object that needs to be added to the KiCoKalender")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(Asset), Summary = "New asset added", Description = "New asset added", Example = typeof(DummyAssetExample))]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid input", Description = "Invalid input")]
        [UnauthorizedResponse]
        [ForbiddenResponse]
        public async Task<HttpResponseData> AddAsset(
            [HttpTrigger(AuthorizationLevel.Anonymous,
            "POST", Route = "asset")]
            HttpRequestData req,
            string folderId,
            FunctionContext executionContext)
        {
            return await Authenticate.ExecuteForUser(req, executionContext, async (ClaimsPrincipal User) =>
            {
                HttpResponseData response = req.CreateResponse(HttpStatusCode.Created);
                try
                {
                    var parsedFile = MultipartFormDataParser.ParseAsync(req.Body);                  

                    if (parsedFile is null)
                    {
                        response = req.CreateResponse(HttpStatusCode.BadRequest);
                    }
                    else
                    {
                        Asset addedAsset = AssetService.AddAsset(parsedFile.Result.Files.First(), Guid.Parse(folderId)).Result;
                        if (addedAsset is null)
                        {
                            response = req.CreateResponse(HttpStatusCode.BadRequest);
                        }
                        else
                        {
                            await response.WriteAsJsonAsync(addedAsset);
                        }
        
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError("Invalid input", ex);
                    response = req.CreateResponse(HttpStatusCode.BadRequest);
                }

                return response;
            });
        }

        [Function("FindAssetsByFolderId")]
        [UserAuth]
        [OpenApiOperation(operationId: "FindAssetsByFolderId", tags: new[] { "asset" }, Summary = "Find assets by folder", Description = "Returns assets by familyId and folder.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "folderId", In = ParameterLocation.Query, Required = true, Type = typeof(Guid), Summary = "FolderId of Assets to return", Description = "FolderId of Assets to return", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IEnumerable<Asset>), Summary = "successful operation", Description = "successful operation", Example = typeof(DummyAssetExample))]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Assets not found", Description = "Assets not found")]
        [UnauthorizedResponse]
        [ForbiddenResponse]
        public async Task<HttpResponseData> FindAssetsByFolderId(
            [HttpTrigger(AuthorizationLevel.Anonymous,
            "GET", Route = "asset")]
            HttpRequestData req,
            string folderId,
            FunctionContext executionContext)
        {
            return await Authenticate.ExecuteForUser(req, executionContext, async (ClaimsPrincipal User) =>
            {
                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

                IEnumerable<AssetDTO> assets = AssetService.FindAssetsDTOByFolderId(Guid.Parse(folderId));

                if (assets.Any())
                {
                    await response.WriteAsJsonAsync(assets);
                }
                else
                {
                    response = req.CreateResponse(HttpStatusCode.NotFound);
                }
                return response;
            });
        }

        [Function("DeleteAsset")]
        [UserAuth]
        [OpenApiOperation(operationId: "DeleteAsset", tags: new[] { "asset" }, Summary = "Deletes an asset from the KiCoKalender", Description = "This Deletes an asset from the KiCoKalender.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "id", In = ParameterLocation.Query, Required = true, Type = typeof(Guid), Summary = "Id of Assets to delete", Description = "Id of Assets to delete", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Asset), Summary = "Asset Deleted", Description = "Asset deleted")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid input", Description = "Invalid input")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Asset not found", Description = "Asset not found")]
        [UnauthorizedResponse]
        [ForbiddenResponse]
        public async Task<HttpResponseData> DeleteAsset(
            [HttpTrigger(AuthorizationLevel.Anonymous,
            "DELETE", Route = "asset")]
            HttpRequestData req,
            string id,
            FunctionContext executionContext)
        {
            return await Authenticate.ExecuteForUser(req, executionContext, async (ClaimsPrincipal User) =>
            {
                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

                Asset deletedAsset = AssetService.DeleteAsset(Guid.Parse(id)).Result;

                if (deletedAsset is null)
                {
                    response = req.CreateResponse(HttpStatusCode.NotFound);
                }

                return response;
            });
        }

        [Function("UpdateAsset")]
        [UserAuth]
        [OpenApiOperation(operationId: "UpdateAsset", tags: new[] { "asset" }, Summary = "Update an existing Asset", Description = "This updates an existing Asset.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "id", In = ParameterLocation.Query, Required = true, Type = typeof(Guid), Summary = "Id of Assets to return", Description = "Id of Assets to return", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "multipart/form-data", bodyType: typeof(TransactionUpdateDTO), Description = "Parameters", Required = true)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Asset), Summary = "Asset details updated", Description = "Asset details updated", Example = typeof(DummyAssetExample))]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Asset not found", Description = "Asset not found")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        [UnauthorizedResponse]
        [ForbiddenResponse]
        public async Task<HttpResponseData> UpdateAsset(
            [HttpTrigger(AuthorizationLevel.Anonymous, "PUT", Route = "asset")]
            HttpRequestData req,
            string id,
            FunctionContext executionContext)
        {
            return await Authenticate.ExecuteForUser(req, executionContext, async (ClaimsPrincipal User) =>
            {
                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
                try
                {
                    var parsedFormBody = await MultipartFormDataParser.ParseAsync(req.Body);
                    var parameters = parsedFormBody.Parameters;

                    AssetUpdateDTO assetUpdate = new()
                    {
                        Name = parameters.FirstOrDefault(x => x.Name == "name").Data,
                        Description = parameters.FirstOrDefault(x => x.Name == "description").Data,
                    };

                    Asset updatedAsset = AssetService.UpdateAsset(assetUpdate, Guid.Parse(id));

                    if (updatedAsset is null)
                    {
                        response = req.CreateResponse(HttpStatusCode.BadRequest);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError("Invalid input", ex);
                    response = req.CreateResponse(HttpStatusCode.BadRequest);
                }

                return response;
            });
        }
    }
}
