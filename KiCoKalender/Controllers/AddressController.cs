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

namespace Controllers
{
    class AddressController
    {
		public ILogger Logger { get; }
		IAddressService AddressService { get; }
		IAuthenticate Authenticate { get; }
		public AddressController(ILogger<AddressController> Logger, IAddressService addressService, IAuthenticate authenticate)
		{
			this.Logger = Logger;
			AddressService = addressService;
			Authenticate = authenticate;
		}

		[Function("AddAddress")]
		[UserAuth]
		[OpenApiOperation(operationId: "AddAddress", tags: new[] { "address" }, Summary = "Add an address to the KiCoKalender", Description = "This adds a new address to the KiCoKalender.", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Address), Required = true, Description = "Address that needs to be added to the KiCoKalender")]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Address), Summary = "New address added", Description = "New address added", Example = typeof(DummyAddressExample))]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid input", Description = "Invalid input")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
		[UnauthorizedResponse]
		[ForbiddenResponse]
		public async Task<HttpResponseData> AddAddress(
			[HttpTrigger(AuthorizationLevel.Function, "POST", Route = "address")]
			HttpRequestData req,
			FunctionContext executionContext)
		{
			return await Authenticate.ExecuteForUser(req, executionContext, async (ClaimsPrincipal User) => {
			// Parse input
			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			Address address = JsonConvert.DeserializeObject<Address>(requestBody);

			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

			if (address is null)
			{
				response = req.CreateResponse(HttpStatusCode.BadRequest);
			}
			else
			{
				AddressService.AddAddress(address);
			}

			await response.WriteAsJsonAsync(address);

			return response;
			});
		}

		[Function("FindAddressByUserId")]
		[UserAuth]
		[OpenApiOperation(operationId: "FindAddressByUserId", tags: new[] { "address" }, Summary = "Find addresses by userId", Description = "Returns addresses.", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "userId for addresses to return", Description = "userId for addresses to return", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IEnumerable<Address>), Summary = "successful operation", Description = "successful operation", Example = typeof(DummyAddressExample))]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Address not found", Description = "Address not found")]
		[UnauthorizedResponse]
		[ForbiddenResponse]
		public async Task<HttpResponseData> FindAddressByUserId(
			[HttpTrigger(AuthorizationLevel.Function,
			"GET", Route = "address/{userId}")]
			HttpRequestData req,
			long userId,
			FunctionContext executionContext)
		{
			return await Authenticate.ExecuteForUser(req, executionContext, async (ClaimsPrincipal User) => {
			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

			await response.WriteAsJsonAsync(AddressService.FindAddressByUserId(userId));

			return response;
			});
		}

		[Function("FindAddressByFamilyId")]
		[UserAuth]
		[OpenApiOperation(operationId: "FindAddressByFamilyId", tags: new[] { "address" }, Summary = "Find addresses by familyId", Description = "Returns addresses.", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiParameter(name: "familyId", In = ParameterLocation.Query, Required = true, Type = typeof(long), Summary = "familyId for addresses to return", Description = "familyId for addresses to return", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IEnumerable<Address>), Summary = "successful operation", Description = "successful operation", Example = typeof(DummyAddressExample))]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "User not found", Description = "User not found")]
		[UnauthorizedResponse]
		[ForbiddenResponse]
		public async Task<HttpResponseData> FindAddressByFamilyId(
			[HttpTrigger(AuthorizationLevel.Function,
			"GET", Route = "address")]
			HttpRequestData req,
			long familyId,
			FunctionContext executionContext)
		{
			return await Authenticate.ExecuteForUser(req, executionContext, async (ClaimsPrincipal User) => {
			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

			await response.WriteAsJsonAsync(AddressService.FindAddressByFamilyId(familyId));

			return response;
			});
		}

		[Function("DeleteAddress")]
		[UserAuth]
		[OpenApiOperation(operationId: "DeleteAddress", tags: new[] { "address" }, Summary = "Deletes a address from the KiCoKalender", Description = "Deletes a address from the KiCoKalender.", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Address), Required = true, Description = "Address that needs to be Deleted from the KiCoKalender")]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Address), Summary = "New address Delete", Description = "Address deleted", Example = typeof(DummyAddressExample))]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid input", Description = "Invalid input")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Address not found", Description = "Address not found")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
		[UnauthorizedResponse]
		[ForbiddenResponse]
		public async Task<HttpResponseData> DeleteAddress(
			[HttpTrigger(AuthorizationLevel.Function,
			"DELETE", Route = "address")]
			HttpRequestData req,
			FunctionContext executionContext)
		{
			return await Authenticate.ExecuteForUser(req, executionContext, async (ClaimsPrincipal User) => {
			// Parse input
			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			Address address = JsonConvert.DeserializeObject<Address>(requestBody);

			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

			if (address is null)
			{
				response = req.CreateResponse(HttpStatusCode.BadRequest);
			}
			else
			{
				AddressService.DeleteAddress(address);
			}

			await response.WriteAsJsonAsync(address);

			return response;
			});
		}

		[Function("UpdateAddress")]
		[UserAuth]
		[OpenApiOperation(operationId: "UpdateAddress", tags: new[] { "address" }, Summary = "Update an existing address", Description = "This updates an existing address.", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Address), Required = true, Description = "Address that needs to be updated")]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Address), Summary = "Address details updated", Description = "Address details updated", Example = typeof(DummyAddressExample))]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Address not found", Description = "Address not found")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
		[UnauthorizedResponse]
		[ForbiddenResponse]
		public async Task<HttpResponseData> UpdateAddress(
			[HttpTrigger(AuthorizationLevel.Function, "PUT", Route = "address")]
			HttpRequestData req,
			FunctionContext executionContext)
		{
			return await Authenticate.ExecuteForUser(req, executionContext, async (ClaimsPrincipal User) => {
			// Parse input
			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			Address address = JsonConvert.DeserializeObject<Address>(requestBody);

			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

			if (address is null)
			{
				response = req.CreateResponse(HttpStatusCode.BadRequest);
			}
			else
			{
				AddressService.UpdateAddress(address);
			}

			await response.WriteAsJsonAsync(address);

			return response;
			});
		}
	}
}
