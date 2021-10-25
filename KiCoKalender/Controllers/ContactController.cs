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
using Auth.Interfaces;
using System.Security.Claims;
using Attributes;
using System;

namespace Controllers
{
    class ContactController
    {
        public ILogger Logger { get; }
        IContactService ContactService { get; }
        IAuthenticate Authenticate { get; }
        public ContactController(ILogger<ContactController> Logger, IContactService contactService, IAuthenticate authenticate)
        {
            this.Logger = Logger;
            ContactService = contactService;
            Authenticate = authenticate;
        }

        [Function("AddContact")]
        [UserAuth]
        [OpenApiOperation(operationId: "AddContact", tags: new[] { "contact" }, Summary = "Add an contact to the KiCoKalender", Description = "This adds a new contact to the KiCoKalender.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Contact), Required = true, Description = "Contact that needs to be added to the KiCoKalender")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(Contact), Summary = "New contact added", Description = "New contact added", Example = typeof(DummyContactExample))]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid input", Description = "Invalid input")]
        [UnauthorizedResponse]
        [ForbiddenResponse]
        public async Task<HttpResponseData> AddAddress(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "contact")]
            HttpRequestData req,
            FunctionContext executionContext)
        {
            return await Authenticate.ExecuteForUser(req, executionContext, async (ClaimsPrincipal User) =>
            {
                HttpResponseData response = req.CreateResponse(HttpStatusCode.Created);
                try
                {
                    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                    Contact contact = JsonConvert.DeserializeObject<Contact>(requestBody);

                    if (contact is null)
                    {
                        response = req.CreateResponse(HttpStatusCode.BadRequest);
                    }
                    else
                    {
                        Contact addedContact = ContactService.AddContact(contact);
                        await response.WriteAsJsonAsync(addedContact);
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

        [Function("FindContactByFamilyId")]
        [UserAuth]
        [OpenApiOperation(operationId: "FindContactByFamilyId", tags: new[] { "contact" }, Summary = "Find contacts by familyId", Description = "Returns contacts.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "familyId", In = ParameterLocation.Query, Required = true, Type = typeof(Guid), Summary = "familyId for contacts to return", Description = "familyId for contacts to return", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IEnumerable<Contact>), Summary = "successful operation", Description = "successful operation", Example = typeof(DummyContactExample))]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Contact not found", Description = "Contact not found")]
        [UnauthorizedResponse]
        [ForbiddenResponse]
        public async Task<HttpResponseData> FindAddressByFamilyId(
            [HttpTrigger(AuthorizationLevel.Anonymous,
            "GET", Route = "contact")]
            HttpRequestData req,
            string familyId,
            FunctionContext executionContext)
        {
            return await Authenticate.ExecuteForUser(req, executionContext, async (ClaimsPrincipal User) =>
            {
                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

                // Generate output
                IEnumerable<Contact> contacts = ContactService.FindContactByFamilyId(Guid.Parse(familyId));

                if (contacts is not null)
                {
                    await response.WriteAsJsonAsync(contacts);
                }
                else
                {
                    response = req.CreateResponse(HttpStatusCode.NotFound);
                }

                return response;
            });
        }

        [Function("DeleteContact")]
        [UserAuth]
        [OpenApiOperation(operationId: "DeleteContact", tags: new[] { "contact" }, Summary = "Deletes a contact from the KiCoKalender", Description = "Deletes a contact from the KiCoKalender.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "Id", In = ParameterLocation.Query, Required = true, Type = typeof(Guid), Summary = "Id of contact to return", Description = "Id of contact to return", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Contact), Summary = "New contact Delete", Description = "Contact deleted", Example = typeof(DummyContactExample))]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid input", Description = "Invalid input")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Contact not found", Description = "Contact not found")]
        [UnauthorizedResponse]
        [ForbiddenResponse]
        public async Task<HttpResponseData> DeleteAddress(
            [HttpTrigger(AuthorizationLevel.Anonymous,
            "DELETE", Route = "contact")]
            HttpRequestData req,
            string id,
            FunctionContext executionContext)
        {
            return await Authenticate.ExecuteForUser(req, executionContext, async (ClaimsPrincipal User) =>
            {
                // Generate output
                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

                Contact deletedContact = ContactService.DeleteContact(Guid.Parse(id));

                if (deletedContact is null)
                {
                    response = req.CreateResponse(HttpStatusCode.NotFound);
                }

                return response;
            });
        }

        [Function("UpdateContact")]
        [UserAuth]
        [OpenApiOperation(operationId: "UpdateContact", tags: new[] { "contact" }, Summary = "Update an existing contact", Description = "This updates an existing contact.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "id", In = ParameterLocation.Query, Required = true, Type = typeof(Guid), Summary = "Id of contact to return", Description = "Id of contact to return", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Contact), Required = true, Description = "Contact that needs to be updated")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Contact), Summary = "Contact details updated", Description = "Contact details updated", Example = typeof(DummyContactExample))]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Contact not found", Description = "Contact not found")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        [UnauthorizedResponse]
        [ForbiddenResponse]
        public async Task<HttpResponseData> UpdateAddress(
            [HttpTrigger(AuthorizationLevel.Anonymous, "PUT", Route = "contact")]
            HttpRequestData req,
            string id,
            FunctionContext executionContext)
        {
            return await Authenticate.ExecuteForUser(req, executionContext, async (ClaimsPrincipal User) =>
            {
                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
                try
                {
                    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                    Contact contact = JsonConvert.DeserializeObject<Contact>(requestBody);

                    Contact updatedContact = ContactService.UpdateContact(contact, Guid.Parse(id));

                    if (updatedContact is null)
                    {
                        response = req.CreateResponse(HttpStatusCode.NotFound);
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
