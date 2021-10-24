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
	[OpenApiExample(typeof(DummyLoginExample))]
	public class Login
	{
		[OpenApiProperty(Description = "Username for the user logging in.")]
		[JsonRequired]
		public string Email { get; set; }

		[OpenApiProperty(Description = "Password for the user logging in.")]
		[JsonRequired]
		public string Password { get; set; }

		public Login()
		{
		
		}

		public Login(string email, string password) 
		{
			Email = email;
			Password = password;
		}

	}

	public class DummyLoginExample : OpenApiExample<Login>
	{
		public override IOpenApiExample<Login> Build(NamingStrategy NamingStrategy = null)
		{
			Examples.Add(OpenApiExampleResolver.Resolve("Login",
														new Login()
														{
															Email = "inlog.email@gmail.com",
															Password = "SuperSecretPassword123!!",
															
														},
														NamingStrategy));

			return this;
		}
	}

}