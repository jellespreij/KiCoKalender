using System.Net;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;


namespace KiCoKalender.Attributes
{
	public class UnauthorizedResponseAttribute : OpenApiResponseWithBodyAttribute
	{
		public UnauthorizedResponseAttribute() : base(HttpStatusCode.Unauthorized, "text/plain", typeof(string))
		{
			this.Description = "User login is invalid.";
		}
	}
}
