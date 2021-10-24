﻿using Microsoft.Extensions.Logging;
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
using Attributes;

namespace Controllers
{
    class TransactionController
    {
        ILogger Logger { get; }
        ITransactionService TransactionService { get; }
        IAuthenticate Authenticate { get; }
        public TransactionController(ILogger<TransactionController> Logger, ITransactionService transactionService, IAuthenticate authenticate)
        {
            this.Logger = Logger;
            TransactionService = transactionService;
            Authenticate = authenticate;
        }

        [Function("AddTransaction")]
        [UserAuth]
        [OpenApiOperation(operationId: "AddTransaction", tags: new[] { "transaction" }, Summary = "Add a transaction to the KiCoKalender", Description = "This adds a transaction to the KiCoKalender.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "localUrl", In = ParameterLocation.Query, Required = true, Type = typeof(string), Summary = "Url of image", Description = "Url of image", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Transaction), Required = true, Description = "Transaction object that needs to be added to the KiCoKalender", Example = typeof(DummyTransactionExample))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Transaction), Summary = "New transaction added", Description = "New transaction added")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Invalid input", Description = "Invalid input")]
        [UnauthorizedResponse]
        [ForbiddenResponse]
        public async Task<HttpResponseData> AddTransaction(
            [HttpTrigger(AuthorizationLevel.Function,
            "POST", Route = "transaction")]
            HttpRequestData req,
            string localUrl,
            FunctionContext executionContext)
        {
            return await Authenticate.ExecuteForUser(req, executionContext, async (ClaimsPrincipal User) =>
            {
                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
                try
                {

                    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                    Transaction transaction = JsonConvert.DeserializeObject<Transaction>(requestBody);

                    if (transaction is null)
                    {
                        response = req.CreateResponse(HttpStatusCode.BadRequest);
                    }
                    else
                    {
                        Transaction addedTransaction = TransactionService.AddTransaction(transaction, localUrl).Result;
                        await response.WriteAsJsonAsync(addedTransaction);
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

        [Function("FindTransactionsByFamilyId")]
        [UserAuth]
        [OpenApiOperation(operationId: "FindTransactionsByFamilyId", tags: new[] { "transaction" }, Summary = "Find transaction by familyId", Description = "Returns transaction by familyId.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "familyId", In = ParameterLocation.Query, Required = true, Type = typeof(Guid), Summary = "familyId of transaction to return", Description = "familyId of transaction to return", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IEnumerable<Transaction>), Summary = "successful operation", Description = "successful operation", Example = typeof(DummyTransactionExample))]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid ID supplied", Description = "Invalid ID supplied")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Assets not found", Description = "Assets not found")]
        [UnauthorizedResponse]
        [ForbiddenResponse]
        public async Task<HttpResponseData> FindTransactionsByFamilyId(
            [HttpTrigger(AuthorizationLevel.Function,
            "GET", Route = "transaction")]
            HttpRequestData req,
            string familyId,
            FunctionContext executionContext)
        {
            return await Authenticate.ExecuteForUser(req, executionContext, async (ClaimsPrincipal User) =>
            {
                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

                // Generate output
                IEnumerable<Transaction> transactions = TransactionService.FindTransactionByFamilyId(Guid.Parse(familyId));

                if (transactions is not null)
                {
                    await response.WriteAsJsonAsync(transactions);
                }
                else
                {
                    response = req.CreateResponse(HttpStatusCode.NotFound);
                }

                return response;
            });
        }

        [Function("DeleteTransaction")]
        [UserAuth]
        [OpenApiOperation(operationId: "DeleteTransaction", tags: new[] { "transaction" }, Summary = "Deletes a transaction from the KiCoKalender", Description = "This Delete a transactions from the KiCoKalender.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "id", In = ParameterLocation.Query, Required = true, Type = typeof(Guid), Summary = "Id of transaction to delete", Description = "Id of transaction to delete", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Transaction), Summary = "Asset Deleted", Description = "Asset deleted")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid input", Description = "Invalid input")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Asset not found", Description = "Asset not found")]
        [UnauthorizedResponse]
        [ForbiddenResponse]
        public async Task<HttpResponseData> DeleteTransaction(
            [HttpTrigger(AuthorizationLevel.Function,
            "DELETE", Route = "transaction")]
            HttpRequestData req,
            string id,
            FunctionContext executionContext)
        {
            return await Authenticate.ExecuteForUser(req, executionContext, async (ClaimsPrincipal User) =>
            {
                // Generate output
                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

                Transaction deletedTransaction = TransactionService.DeleteTransaction(Guid.Parse(id)).Result;

                if (deletedTransaction is null)
                {
                    response = req.CreateResponse(HttpStatusCode.NotFound);
                }

                return response;
            });
        }

        [Function("UpdateTransaction")]
        [UserAuth]
        [OpenApiOperation(operationId: "UpdateTransaction", tags: new[] { "transaction" }, Summary = "Update an existing transaction", Description = "This updates an existing transaction.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "id", In = ParameterLocation.Query, Required = true, Type = typeof(Guid), Summary = "Id of transaction to update", Description = "Id of transaction to update", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(Asset), Required = true, Description = "Asset object that needs to be updated")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Asset), Summary = "Transaction details updated", Description = "Transaction details updated", Example = typeof(DummyTransactionExample))]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "Asset not found", Description = "Asset not found")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.MethodNotAllowed, Summary = "Validation exception", Description = "Validation exception")]
        [UnauthorizedResponse]
        [ForbiddenResponse]
        public async Task<HttpResponseData> UpdateAsset(
            [HttpTrigger(AuthorizationLevel.Function, "PUT", Route = "transaction")]
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
                    Transaction transaction = JsonConvert.DeserializeObject<Transaction>(requestBody);

                    Transaction updatedTransaction = TransactionService.UpdateTransaction(transaction, Guid.Parse(id));

                    if (updatedTransaction is null)
                    {
                        response = req.CreateResponse(HttpStatusCode.NotFound);
                    }

                }
                catch(Exception ex)
                {
                    Logger.LogError("Invalid input", ex);
                    response = req.CreateResponse(HttpStatusCode.BadRequest);
                }

                return response;
            });
        }
    }
}
