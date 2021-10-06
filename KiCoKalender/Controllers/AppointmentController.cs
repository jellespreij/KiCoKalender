using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Models;
using Services;
using System.IO;
using Newtonsoft.Json;

namespace Controllers
{
    class AppointmentController
    {
		ILogger Logger { get; }
		IAppointmentService AppointmentService { get; }

        public AppointmentController(ILogger<AppointmentController> Logger, IAppointmentService appointmentService)
        {
            this.Logger = Logger;
            AppointmentService = appointmentService;
        }

		[Function("AddAppointment")]
		[OpenApiOperation(operationId: "AddAppointment", tags: new[] { "appointment" }, Summary = "Add an appointment to the KiCoKalender", Description = "This adds an appointment to the KiCoKalender.", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Appointment), Required = true, Description = "Appointment that needs to be added to the KiCoKalender")]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Appointment), Summary = "New appointment added", Description = "New appointment added", Example = typeof(DummyAppointmentExample))]
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

			if (appointment is null)
			{
				response = req.CreateResponse(HttpStatusCode.BadRequest);
			}
			else
			{
				AppointmentService.AddAppointment(appointment);
			}

			return response;
		}

		[Function("FindAppointmentByUserId")]
		[OpenApiOperation(operationId: "FindAppointmentByUserId", tags: new[] { "appointment" }, Summary = "Find appointments by userId", Description = "Returns appointments.", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(long), Summary = "userId for appointments to return", Description = "userId for appointments to return", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IEnumerable<Appointment>), Summary = "successful operation", Description = "successful operation", Example = typeof(DummyAppointmentExample))]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "User not found", Description = "User not found")]
		public async Task<HttpResponseData> FindAppointmentByUserId(
			[HttpTrigger(AuthorizationLevel.Function,
			"GET", Route = "appointment/{userId}")]
			HttpRequestData req,
			long userId,
			FunctionContext executionContext)
		{
			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

			await response.WriteAsJsonAsync(AppointmentService.FindAppointmentByUserId(userId));

			return response;
		}

		[Function("FindAppointmentByFamilyId")]
		[OpenApiOperation(operationId: "FindAppointmentByFamilyId", tags: new[] { "appointment" }, Summary = "Find appointments by familyId", Description = "Returns appointments.", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiParameter(name: "familyId", In = ParameterLocation.Query, Required = true, Type = typeof(long), Summary = "familyId for appointments to return", Description = "familyId for appointments to return", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IEnumerable<Appointment>), Summary = "successful operation", Description = "successful operation", Example = typeof(DummyAppointmentExample))]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
		[OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "User not found", Description = "User not found")]
		public async Task<HttpResponseData> FindAppointmentByFamilyId(
			[HttpTrigger(AuthorizationLevel.Function,
			"GET", Route = "appointment")]
			HttpRequestData req,
			long familyId,
			FunctionContext executionContext)
		{
			// Generate output
			HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

			await response.WriteAsJsonAsync(AppointmentService.FindAppointmentByFamilyId(familyId));
			
			return response;
		}

		[Function("DeleteAppointment")]
		[OpenApiOperation(operationId: "DeleteAppointment", tags: new[] { "appointment" }, Summary = "Deletes an appointment from the KiCoKalender", Description = "This Deletes an appointment from the KiCoKalender.", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Appointment), Required = true, Description = "appointment that needs to be Deleted from the KiCoKalender")]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Appointment), Summary = "Apointment Deleted", Description = "Appointment deleted", Example = typeof(DummyAppointmentExample))]
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

			if (appointment is null)
			{
				response = req.CreateResponse(HttpStatusCode.BadRequest);
			}
			else
			{
				AppointmentService.DeleteAppointment(appointment);
			}

			return response;
		}

		[Function("UpdateAppointment")]
		[OpenApiOperation(operationId: "UpdateAppointment", tags: new[] { "appointment" }, Summary = "Update an existing appointment", Description = "This updates an existing appointment.", Visibility = OpenApiVisibilityType.Important)]
		[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Appointment), Required = true, Description = "appointment that needs to be updated")]
		[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Appointment), Summary = "appointment details updated", Description = "appointment details updated", Example = typeof(DummyAppointmentExample))]
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

			if (appointment is null)
			{
				response = req.CreateResponse(HttpStatusCode.BadRequest);
			}
			else
			{
				AppointmentService.UpdateAppointment(appointment);
			}

			await response.WriteAsJsonAsync(appointment);

			return response;
		}
	}
}
