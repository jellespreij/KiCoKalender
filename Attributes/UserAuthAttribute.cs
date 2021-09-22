using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;

namespace KiCoKalender.Attributes
{
    public class UserAuthAttribute : OpenApiSecurityAttribute
    {
		public UserAuthAttribute() : base("UserAuth", SecuritySchemeType.Http)
		{
			Description = "JWT for authorization";
			In = OpenApiSecurityLocationType.Header;
			Scheme = OpenApiSecuritySchemeType.Bearer;
			BearerFormat = "JWT";
		}
	}
}
