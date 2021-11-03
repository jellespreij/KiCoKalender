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
using Auth.Attributes;
using Services.Interfaces;

namespace KiCoKalender.Controllers
{
    public class FamilyController
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
        [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Summary = "UserId of user to add to the KiCoKalender", Description = "UserId of user to add to the KiCoKalender", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Family), Required = true, Description = "Family that needs to be added to the KiCoKalender", Example = typeof(DummyFamilyExample))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(Family), Summary = "New family added", Description = "New family added")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid input", Description = "Invalid input")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Conflict, Summary = "Family already exists", Description = "Family already exists")]
        [UnauthorizedResponse]
        [ForbiddenResponse]
        public async Task<HttpResponseData> AddFamily(
            [HttpTrigger(AuthorizationLevel.Anonymous,
            "POST", Route = "family/{userId}")]
            HttpRequestData req,
            string userId,
            FunctionContext executionContext)
        {
            return await Authenticate.ExecuteForUser(req, executionContext, async (ClaimsPrincipal User) =>
            {
                HttpResponseData response = req.CreateResponse(HttpStatusCode.Created);
                try
                {
                    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                    Family family = JsonConvert.DeserializeObject<Family>(requestBody);

                    if (family is null)
                    {
                        response = req.CreateResponse(HttpStatusCode.BadRequest);
                    }
                    else
                    {
                        Family addedFamily = FamilyService.AddFamily(family, Guid.Parse(userId));
                        await response.WriteAsJsonAsync(addedFamily);
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

        [Function("FindFamilyByFamilyId")]
        [UserAuth]
        [OpenApiOperation(operationId: "FindFamilyByFamilyId", tags: new[] { "family" }, Summary = "Find family by familyId", Description = "Returns a family by familyId.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "familyId", In = ParameterLocation.Query, Required = true, Type = typeof(Guid), Summary = "FamilyId of family to return", Description = "FamilyId of family to return", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Family), Summary = "successful operation", Description = "successful operation", Example = typeof(DummyFamilyExample))]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid Id supplied", Description = "Invalid Id supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "User not found", Description = "User not found")]
        [UnauthorizedResponse]
        [ForbiddenResponse]
        public async Task<HttpResponseData> FindFamilyByFamilyId(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "family")]
            HttpRequestData req,
            string familyId,
            FunctionContext executionContext)
        {
            return await Authenticate.ExecuteForUser(req, executionContext, async (ClaimsPrincipal User) =>
            {

                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

                Family family = FamilyService.FindFamilyByFamilyId(Guid.Parse(familyId));

                if (family is not null)
                {
                    await response.WriteAsJsonAsync(family);
                }
                else
                {
                    response = req.CreateResponse(HttpStatusCode.NotFound);
                }

                return response;
            });
        }

        [Function("InsertUserIntoFamily")]
        [UserAuth]
        [OpenApiOperation(operationId: "InsertUserIntoFamily", tags: new[] { "family" }, Summary = "Update an existing family", Description = "This updates an existing family.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "id", In = ParameterLocation.Query, Required = true, Type = typeof(Guid), Summary = "Id of family to return", Description = "Id of family to return", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(User), Required = true, Description = "Family that needs to be updated")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(User), Summary = "Family details updated", Description = "Family details updated", Example = typeof(DummyFamilyExample))]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Family not found", Description = "Family not found")]
        [UnauthorizedResponse]
        [ForbiddenResponse]
        public async Task<HttpResponseData> InsertUserIntoFamily(
            [HttpTrigger(AuthorizationLevel.Anonymous, "PUT", Route = "family/add-user")]
            HttpRequestData req,
            string id,
            FunctionContext executionContext)
        {
            return await Authenticate.ExecuteForUser(req, executionContext, async (ClaimsPrincipal User) =>
            {

                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

                try
                {
                    // Parse input
                    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                    User user = JsonConvert.DeserializeObject<User>(requestBody);
                    // Generate output

                    if (user is null)
                    {
                        response = req.CreateResponse(HttpStatusCode.BadRequest);
                    }
                    else
                    {
                        FamilyService.AddUserToFamily(user, Guid.Parse(id));
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

        [Function("InsertFolderIntoFamily")]
        [UserAuth]
        [OpenApiOperation(operationId: "InsertFolderIntoFamily", tags: new[] { "family" }, Summary = "Update an existing family", Description = "This updates an existing family.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "id", In = ParameterLocation.Query, Required = true, Type = typeof(Guid), Summary = "Id of family to update", Description = "Id of family to update", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Folder), Required = true, Description = "Folder taht needs to be added to the family")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Folder), Summary = "Family details updated", Description = "Family details updated", Example = typeof(DummyFamilyExample))]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Family not found", Description = "Family not found")]
        [UnauthorizedResponse]
        [ForbiddenResponse]
        public async Task<HttpResponseData> InsertFolderIntoFamily(
            [HttpTrigger(AuthorizationLevel.Anonymous, "PUT", Route = "family/add-folder")]
            HttpRequestData req,
            string id,
            FunctionContext executionContext)
        {
            return await Authenticate.ExecuteForUser(req, executionContext, async (ClaimsPrincipal User) =>
            {
                // Parse input
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                Folder folder = JsonConvert.DeserializeObject<Folder>(requestBody);

                // Generate output
                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

                if (folder is null)
                {
                    response = req.CreateResponse(HttpStatusCode.BadRequest);
                }
                else
                {
                    FamilyService.AddFolderToFamily(folder, Guid.Parse(id));
                }

                return response;
            });
        }

        [Function("Deletefamily")]
        [UserAuth]
        [OpenApiOperation(operationId: "deleteFamily", tags: new[] { "family" }, Summary = "Deletes a family from the KiCoKalender", Description = "Deletes a family from the KiCoKalender.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "id", In = ParameterLocation.Query, Required = true, Type = typeof(Guid), Summary = "Id of family to return", Description = "Id of family to return", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Family), Summary = "Family Delete", Description = "Family deleted", Example = typeof(DummyFamilyExample))]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid input", Description = "Invalid input")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Family not found", Description = "Family not found")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        [UnauthorizedResponse]
        [ForbiddenResponse]
        public async Task<HttpResponseData> DeleteUser(
           [HttpTrigger(AuthorizationLevel.Anonymous,
            "DELETE", Route = "family")]
            HttpRequestData req,
           string id,
           FunctionContext executionContext)
        {
            return await Authenticate.ExecuteForUser(req, executionContext, async (ClaimsPrincipal User) =>
            {
                // Generate output
                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

                Family deletedFamily = FamilyService.DeleteFamily(Guid.Parse(id));

                if (deletedFamily is null)
                {
                    response = req.CreateResponse(HttpStatusCode.NotFound);
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
            return await Authenticate.ExecuteForUser(req, executionContext, async (ClaimsPrincipal User) =>
            {
                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
                FamilyService.SendMail(email);

                return response;
            });
        }
    }
}
