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
    class AppointmentHttpTrigger
    {
        ILogger Logger { get; }
        IAppointmentService IAppointmentService { get; }

        public AppointmentHttpTrigger(ILogger<UserHttpTrigger> Logger, IAppointmentService appointmentService)
        {
            this.Logger = Logger;
            this.IAppointmentService = appointmentService;
        }

		[Function(nameof(AppointmentHttpTrigger.GetAppointmentsByAppointsmentId))]
		[OpenApiOperation(operationId: "GetAppointmentsByAppointsmentId", tags: new[] { "appointment" }, Summary = "Find appointment by appointmentId", Description = "Returns an appointment.", Visibility = OpenApiVisibilityType.Important)]
		//[OpenApiSecurity("petstore_auth", SecuritySchemeType.Http, In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
		[OpenApiParameter(name: "appointmentId", In = ParameterLocation.Path, Required = true, Type = typeof(long?), Summary = "appointmentId for appointments to return", Description = "appointmentId for appointments to return", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Appointment), Summary = "successful operation", Description = "successful operation")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "appointment not found", Description = "appointment not found")]
		public async Task<HttpResponseData> GetAppointmentsByAppointsmentId(
			[HttpTrigger(AuthorizationLevel.Function,
			"GET", Route = "appointment/{appointmentId}")]
			HttpRequestData req,
			long? appointmentId,
			FunctionContext executionContext)
		{
			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

			if (appointmentId.HasValue)
			{
				response = req.CreateResponse(HttpStatusCode.BadRequest);
			}
			else
			{
				//await response.WriteAsJsonAsync(AssetsService.GetAssetsById(assetId));
			}

			return response;
		}

		[Function(nameof(AppointmentHttpTrigger.GetAppointmentsByUserId))]
		[OpenApiOperation(operationId: "GetAppointmentsByUserId", tags: new[] { "appointment" }, Summary = "Find appointment by userId", Description = "Returns an appointment.", Visibility = OpenApiVisibilityType.Important)]
		//[OpenApiSecurity("petstore_auth", SecuritySchemeType.Http, In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
		[OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(long?), Summary = "userId for appointments to return", Description = "userId for appointments to return", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Appointment), Summary = "successful operation", Description = "successful operation")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "User not found", Description = "User not found")]
		public async Task<HttpResponseData> GetAppointmentsByUserId(
			[HttpTrigger(AuthorizationLevel.Function,
			"GET", Route = "appointment/GetAppointmentsByUserId")]
			HttpRequestData req,
			long? userId,
			FunctionContext executionContext)
		{
			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

			if (userId.HasValue)
			{
				response = req.CreateResponse(HttpStatusCode.BadRequest);
			}
			else
			{
				//await response.WriteAsJsonAsync(AssetsService.GetAssetsById(assetId));
			}

			return response;
		}

		[Function(nameof(AppointmentHttpTrigger.AddAppointment))]
		[OpenApiOperation(operationId: "AddAppointment", tags: new[] { "appointment" }, Summary = "Add an appointment to the KiCoKalender", Description = "This adds an appointment to the KiCoKalender.", Visibility = OpenApiVisibilityType.Important)]
		//[OpenApiSecurity("petstore_auth", SecuritySchemeType.Http, In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
		[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Appointment), Required = true, Description = "Appointment object that needs to be added to the KiCoKalender")]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Appointment), Summary = "New appointment added", Description = "New appointment added")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Invalid input", Description = "Invalid input")]
		public async Task<HttpResponseData> AddAppointment(
			[HttpTrigger(AuthorizationLevel.Function,
			"POST", Route = "appointment")]
			HttpRequestData req,
			FunctionContext executionContext)
		{
			// Parse input
			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			Appointment appointment = JsonConvert.DeserializeObject<Appointment>(requestBody);

			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

			if (appointment == null)
			{
				response = req.CreateResponse(HttpStatusCode.BadRequest);
			}
			else
			{
				//UserService.AddAsset(asset);
			}

			return response;
		}

		[Function(nameof(AppointmentHttpTrigger.DeleteAppointment))]
		[OpenApiOperation(operationId: "deleteAppointment", tags: new[] { "appointment" }, Summary = "Deletes an appointment from the KiCoKalender", Description = "This Deletes an appointment from the KiCoKalender.", Visibility = OpenApiVisibilityType.Important)]
		//[OpenApiSecurity("petstore_auth", SecuritySchemeType.Http, In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
		[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Appointment), Required = true, Description = "appointment object that needs to be Deleted from the KiCoKalender")]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Appointment), Summary = "New apointment Delete", Description = "appointment deleted")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Invalid input", Description = "Invalid input")]
		public async Task<HttpResponseData> DeleteAppointment(
			[HttpTrigger(AuthorizationLevel.Function,
			"DELETE", Route = "appointment")]
			HttpRequestData req,
			FunctionContext executionContext)
		{
			// Parse input
			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			Appointment appointment = JsonConvert.DeserializeObject<Appointment>(requestBody);

			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

			if (appointment == null)
			{
				response = req.CreateResponse(HttpStatusCode.BadRequest);
			}
			else
			{
				//UserService.AddAsset(asset);
			}

			return response;
		}

		[Function(nameof(AppointmentHttpTrigger.UpdateAppointment))]
		[OpenApiOperation(operationId: "updateAppointment", tags: new[] { "appointment" }, Summary = "Update an existing appointment", Description = "This updates an existing appointment.", Visibility = OpenApiVisibilityType.Important)]
		//[OpenApiSecurity("petstore_auth", SecuritySchemeType.Http, In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
		[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Appointment), Required = true, Description = "appointment object that needs to be updated")]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Appointment), Summary = "appointment details updated", Description = "appointment details updated")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid appointment supplied", Description = "Invalid appointment supplied")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "appointment not found", Description = "Appointment not found")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
		public async Task<HttpResponseData> UpdateAppointment(
			[HttpTrigger(AuthorizationLevel.Function, "PUT", Route = "appointment")]
			HttpRequestData req,
			FunctionContext executionContext)
		{
			// Parse input
			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			Appointment appointment = JsonConvert.DeserializeObject<Appointment>(requestBody);

			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

			await response.WriteAsJsonAsync(appointment);

			return response;
		}
	}
}

