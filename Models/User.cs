using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
	[OpenApiExample(typeof(UserExample))]
	public class User : IEntityBase
	{
		[OpenApiProperty(Description = "Gets or sets the ID.")]
		[JsonRequired]
		public long Id { get; set; }

		[OpenApiProperty(Description = "Username for the user logging in.")]
		[JsonRequired]
		public string Username { get; set; }

		[OpenApiProperty(Description = "Password for the user logging in.")]
		[JsonRequired]
		public string Password { get; set; }

		[OpenApiProperty(Description = "Gets or sets the partitionKey.")]
		[JsonRequired]
		public string PartitionKey { get; set; }
	}

	public class UserExample : OpenApiExample<User>
	{
		public override IOpenApiExample<User> Build(NamingStrategy NamingStrategy = null)
		{
			Examples.Add(OpenApiExampleResolver.Resolve("Kim",
														new User()
														{
															Username = "Kim",
															Password = "SuperSecretPassword123!!",
															PartitionKey = "1"
														},
														NamingStrategy));

			return this;
		}
	}

}