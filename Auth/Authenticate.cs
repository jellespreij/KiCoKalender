using Auth.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Auth
{
    public class Authenticate : IAuthenticate
    {
		ILogger Logger { get; }

		public Authenticate(ILogger<Authenticate> Logger)
		{
			this.Logger = Logger;
		}
		public async Task<HttpResponseData> ExecuteForUser(HttpRequestData Request, FunctionContext ExecutionContext, Func<ClaimsPrincipal, Task<HttpResponseData>> Delegate)
		{
			try
			{
				ClaimsPrincipal User = ExecutionContext.GetUser();

				if (!User.IsInRole("User"))
				{
					HttpResponseData Response = Request.CreateResponse(HttpStatusCode.Forbidden);

					return Response;
				}
				try
				{
					return await Delegate(User).ConfigureAwait(false);
				}
				catch (Exception e)
				{
					HttpResponseData Response = Request.CreateResponse(HttpStatusCode.BadRequest);
					return Response;
				}
			}
			catch (Exception e)
			{
				Logger.LogError(e.Message);

				HttpResponseData Response = Request.CreateResponse(HttpStatusCode.Unauthorized);
				return Response;
			}
		}
	}
}
