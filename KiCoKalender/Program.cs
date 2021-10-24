using Auth;
using Auth.Interfaces;
using Context;
using Controllers;
using KiCoKalender.Configurations;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Functions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Repositories;
using Services;
using System.IO;

namespace KiCoKalender
{
    public class Program
    {
		public static void Main()
		{
			IHost host = new HostBuilder()
				.ConfigureFunctionsWorkerDefaults((IFunctionsWorkerApplicationBuilder Builder) => {
					Builder.UseNewtonsoftJson().UseMiddleware<JwtMiddleware>();
				})
				.ConfigureOpenApi()
				.ConfigureServices(Configure)
				.Build();


			host.Run();
		}

		static void Configure(HostBuilderContext Builder, IServiceCollection Services)
		{
			Services.AddSingleton<IOpenApiHttpTriggerContext, OpenApiHttpTriggerContext>();
			Services.AddSingleton<IOpenApiTriggerFunction, OpenApiTriggerFunction>();

			Services.AddSingleton<UserController>();
			Services.AddSingleton<IUserService, UserService>();
			Services.AddTransient<IUserRepository, UserRepository>();

			Services.AddSingleton<AppointmentController>();
			Services.AddSingleton<IAppointmentService, AppointmentService>();
			Services.AddTransient<IAppointmentRepository, AppointmentRepository>();

			Services.AddSingleton<AssetController>();
			Services.AddSingleton<IAssetService, AssetService>();
			Services.AddSingleton<BlobService>();
			Services.AddTransient<IAssetRepository, AssetRepository>();

			Services.AddSingleton<FamilyController>();
			Services.AddSingleton<IFamilyService, FamilyService>();
			Services.AddTransient<IFamilyRepository, FamilyRepository>();
			Services.AddTransient<IFolderRepository, FolderRepository>();

			Services.AddSingleton<ContactController>();
			Services.AddSingleton<IContactService, ContactService>();
			Services.AddTransient<IContactRepository, ContactRepository>();

			Services.AddSingleton<TransactionController>();
			Services.AddSingleton<ITransactionService, TransactionService>();
			Services.AddTransient<ITransactionRepository, TransactionRepository>();

			Services.AddSingleton<AuthController>();
			Services.AddSingleton<ITokenService, TokenService>();
			Services.AddSingleton<IAuthenticate, Authenticate>();
			Services.AddTransient<IAuthRepository, AuthRepository>();

			Services.AddTransient<CosmosDBContext>();
		}
	}
}