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
using Auth.Interfaces;
using System.Security.Claims;
using Attributes;
using Microsoft.Azure.Cosmos;

namespace Controllers
{
    class AppointmentController
    {
        ILogger Logger { get; }
        IAppointmentService AppointmentService { get; }
        IAuthenticate Authenticate { get; }

        public AppointmentController(ILogger<AppointmentController> Logger, IAppointmentService appointmentService, IAuthenticate authenticate)
        {
            this.Logger = Logger;
            AppointmentService = appointmentService;
            Authenticate = authenticate;
        }

        [Function("AddAppointment")]
        [UserAuth]
        [OpenApiOperation(operationId: "AddAppointment", tags: new[] { "appointment" }, Summary = "Add an appointment to the KiCoKalender", Description = "This adds an appointment to the KiCoKalender.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Appointment), Required = true, Description = "Appointment that needs to be added to the KiCoKalender")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(Appointment), Summary = "New appointment added", Description = "New appointment added", Example = typeof(DummyAppointmentExample))]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid input", Description = "Invalid input")]
        [UnauthorizedResponse]
        [ForbiddenResponse]
        public async Task<HttpResponseData> AddAppointment(
            [HttpTrigger(AuthorizationLevel.Function,
            "POST", Route = "appointment")]
            HttpRequestData req,
            FunctionContext executionContext)
        {
            return await Authenticate.ExecuteForUser(req, executionContext, async (ClaimsPrincipal User) =>
            {
                HttpResponseData response = req.CreateResponse(HttpStatusCode.Created);

                try
                {
                    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                    Appointment appointment = JsonConvert.DeserializeObject<Appointment>(requestBody);

                    if (appointment is null)
                    {
                        response = req.CreateResponse(HttpStatusCode.BadRequest);
                    }
                    else
                    {
                        Appointment addedAppointment = AppointmentService.AddAppointment(appointment);
                        await response.WriteAsJsonAsync(addedAppointment);
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

        [Function("FindAppointmentByFamilyIdAndUserId")]
        [UserAuth]
        [OpenApiOperation(operationId: "FindAppointmentByFamilyIdAndUserId", tags: new[] { "appointment" }, Summary = "Find appointments by familyId", Description = "Returns appointments.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Summary = "userId for appointments to return", Description = "userId for appointments to return", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "familyId", In = ParameterLocation.Query, Required = true, Type = typeof(Guid), Summary = "familyId for appointments to return", Description = "familyId for appointments to return", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IEnumerable<Appointment>), Summary = "successful operation", Description = "successful operation", Example = typeof(DummyAppointmentExample))]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "User not found", Description = "User not found")]
        [UnauthorizedResponse]
        [ForbiddenResponse]
        public async Task<HttpResponseData> FindAppointmentByFamilyIdAndUserId(
            [HttpTrigger(AuthorizationLevel.Function,
            "GET", Route = "appointment/{userId}")]
            HttpRequestData req,
            string userId,
            string familyId,
            FunctionContext executionContext)
        {
            return await Authenticate.ExecuteForUser(req, executionContext, async (ClaimsPrincipal User) =>
            {
                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

                IEnumerable<Appointment> appointments = AppointmentService.FindAppointmentByFamilyIdAndUserId(Guid.Parse(familyId), Guid.Parse(userId));

                if (appointments.Any())
                {
                    await response.WriteAsJsonAsync(appointments);
                }
                else
                {
                    response = req.CreateResponse(HttpStatusCode.NotFound);
                }

                return response;
            });
        }

        [Function("DeleteAppointment")]
        [UserAuth]
        [OpenApiOperation(operationId: "DeleteAppointment", tags: new[] { "appointment" }, Summary = "Deletes an appointment from the KiCoKalender", Description = "This Deletes an appointment from the KiCoKalender.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "id", In = ParameterLocation.Query, Required = true, Type = typeof(Guid), Summary = "Id of appointemnt to return", Description = "Id of appointment to return", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Appointment), Summary = "Apointment Deleted", Description = "Appointment deleted", Example = typeof(DummyAppointmentExample))]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid input", Description = "Invalid input")]
        [UnauthorizedResponse]
        [ForbiddenResponse]
        public async Task<HttpResponseData> DeleteAppointment(
            [HttpTrigger(AuthorizationLevel.Function,
            "DELETE", Route = "appointment")]
            HttpRequestData req,
            string id,
            FunctionContext executionContext)
        {
            return await Authenticate.ExecuteForUser(req, executionContext, async (ClaimsPrincipal User) =>
            {
                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

                Appointment deletedAppointment = AppointmentService.DeleteAppointment(Guid.Parse(id));

                if (deletedAppointment is null)
                {
                    response = req.CreateResponse(HttpStatusCode.NotFound);
                }              

                return response;
            });
        }

        [Function("UpdateAppointment")]
        [UserAuth]
        [OpenApiOperation(operationId: "UpdateAppointment", tags: new[] { "appointment" }, Summary = "Update an existing appointment", Description = "This updates an existing appointment.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Appointment), Required = true, Description = "appointment that needs to be updated")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Query, Required = true, Type = typeof(Guid), Summary = "Id of appointemnt to return", Description = "Id of appointment to return", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Appointment), Summary = "appointment details updated", Description = "appointment details updated", Example = typeof(DummyAppointmentExample))]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid appointment supplied", Description = "Invalid appointment supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "appointment not found", Description = "Appointment not found")]
        [UnauthorizedResponse]
        [ForbiddenResponse]
        public async Task<HttpResponseData> UpdateAppointment(
            [HttpTrigger(AuthorizationLevel.Function, "PUT", Route = "appointment")]
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
                    Appointment appointment = JsonConvert.DeserializeObject<Appointment>(requestBody);

                    Appointment updatedAppointment = AppointmentService.UpdateAppointment(appointment, Guid.Parse(id));

                    if (updatedAppointment is null)
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
