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
using Auth.Attributes;
using Microsoft.Azure.Cosmos;
using Services.Interfaces;
using HttpMultipartParser;

namespace KiCoKalender.Controllers
{
    public class AppointmentController
    {
        ILogger Logger { get; }
        IAppointmentService AppointmentService { get; }
        IValidationService InputCheckService { get; }
        IAuthenticate Authenticate { get; }

        public AppointmentController(ILogger<AppointmentController> Logger, IAppointmentService appointmentService, IAuthenticate authenticate, IValidationService inputCheckService)
        {
            this.Logger = Logger;
            AppointmentService = appointmentService;
            Authenticate = authenticate;
            InputCheckService = inputCheckService;
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
            [HttpTrigger(AuthorizationLevel.Anonymous,
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
                    string checkedUserInput = InputCheckService.CheckUserInput(requestBody);
                    Appointment appointment = JsonConvert.DeserializeObject<Appointment>(checkedUserInput);

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
            [HttpTrigger(AuthorizationLevel.Anonymous,
            "GET", Route = "appointment/{userId}")]
            HttpRequestData req,
            string userId,
            string familyId,
            FunctionContext executionContext)
        {
            return await Authenticate.ExecuteForUser(req, executionContext, async (ClaimsPrincipal User) =>
            {
                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

                IEnumerable<AppointmentDTO> appointments = AppointmentService.FindAppointmentDTOByFamilyIdAndUserId(Guid.Parse(familyId), Guid.Parse(userId));

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
            [HttpTrigger(AuthorizationLevel.Anonymous,
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
        [OpenApiRequestBody(contentType: "multipart/form-data", bodyType: typeof(AppointmentUpdateDTO), Description = "Parameters", Required = true)]
        [OpenApiParameter(name: "id", In = ParameterLocation.Query, Required = true, Type = typeof(Guid), Summary = "Id of appointemnt to return", Description = "Id of appointment to return", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Appointment), Summary = "appointment details updated", Description = "appointment details updated", Example = typeof(DummyAppointmentExample))]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid appointment supplied", Description = "Invalid appointment supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "appointment not found", Description = "Appointment not found")]
        [UnauthorizedResponse]
        [ForbiddenResponse]
        public async Task<HttpResponseData> UpdateAppointment(
            [HttpTrigger(AuthorizationLevel.Anonymous, "PUT", Route = "appointment")]
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

                    AppointmentUpdateDTO appointmentUpdate = new()
                    {
                        Invited = (Invited)Enum.Parse(typeof(Invited), (parameters.FirstOrDefault(x => x.Name == "invited").Data)),
                        Name = parameters.FirstOrDefault(x => x.Name == "name").Data,
                        Description = parameters.FirstOrDefault(x => x.Name == "description").Data,
                        LocationDropofId = Guid.Parse(parameters.FirstOrDefault(x => x.Name == "locationDropofId").Data),
                        LocationPickupId = Guid.Parse(parameters.FirstOrDefault(x => x.Name == "locationPickupId").Data),
                        LocationId = Guid.Parse(parameters.FirstOrDefault(x => x.Name == "locationId").Data),
                        StartTime = DateTime.Parse(parameters.FirstOrDefault(x => x.Name == "startTime").Data),
                        EndTime = DateTime.Parse(parameters.FirstOrDefault(x => x.Name == "endTime").Data),
                        Date = DateTime.Parse(parameters.FirstOrDefault(x => x.Name == "date").Data),
                        Privacy = bool.Parse(parameters.FirstOrDefault(x => x.Name == "privacy").Data)
                    };

                    Appointment updatedAppointment = AppointmentService.UpdateAppointment(appointmentUpdate, Guid.Parse(id));

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
