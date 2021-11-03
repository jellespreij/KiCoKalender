using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.OpenApi.Models;

namespace Auth.Attributes
{
	public class UserAuthAttribute : OpenApiSecurityAttribute {
		public UserAuthAttribute() : base("UserAuth", SecuritySchemeType.Http) {
			Description = "JWT for authorization";
			In = OpenApiSecurityLocationType.Header;
			Scheme = OpenApiSecuritySchemeType.Bearer;
			BearerFormat = "JWT";
		}
	}
}
