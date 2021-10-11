using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
	[OpenApiExample(typeof(UserExample))]
	public class User : IEntityBase
	{
		[OpenApiProperty(Description = "Gets or sets the ID.")]
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		[OpenApiProperty(Description = "Username for the user logging in.")]
		[JsonRequired]
		public string Username { get; set; }

		[OpenApiProperty(Description = "Password for the user logging in.")]
		[JsonRequired]
		public string Password { get; set; }

		[OpenApiProperty(Description = "Gets or sets the partitionKey.")]
        public string PartitionKey
		{
			get => Id.ToString();
			set => Id = Guid.Parse(value);
		}
	}

	public class UserExample : OpenApiExample<User>
	{
		public override IOpenApiExample<User> Build(NamingStrategy NamingStrategy = null)
		{
			Examples.Add(OpenApiExampleResolver.Resolve("Kim",
														new User()
														{
															Username = "Kim",
															Password = "SuperSecretPassword123!!"
														},
														NamingStrategy));

			return this;
		}
	}

}