using Context;
using Controllers;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Functions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Repositories;
using Services;

namespace KiCoKalender
{
    public class Program
    {
		public static void Main()
		{
			IHost host = new HostBuilder()
				.ConfigureFunctionsWorkerDefaults((IFunctionsWorkerApplicationBuilder Builder) => {
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

			Services.AddSingleton<UserContextController>();
			Services.AddSingleton<IUserContextService, UserContextService>();
			Services.AddTransient<IUserContextRepository, UserContextRepository>();

			Services.AddSingleton<AppointmentController>();
			Services.AddSingleton<IAppointmentService, AppointmentService>();
			Services.AddTransient<IAppointmentRepository, AppointmentRepository>();

			Services.AddSingleton<AssetController>();
			Services.AddSingleton<IAssetService, AssetService>();
			Services.AddTransient<IAssetRepository, AssetRepository>();

			Services.AddSingleton<FamilyController>();
			Services.AddSingleton<IFamilyService, FamilyService>();
			Services.AddTransient<IFamilyRepository, FamilyRepository>();

			Services.AddSingleton<AddressController>();
			Services.AddSingleton<IAddressService, AddressService>();
			Services.AddTransient<IAddressRepository, AddressRepository>();

			Services.AddTransient<CosmosDBContext>();
		}
	}
}