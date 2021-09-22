using KiCoKalender.Interfaces;
using KiCoKalender.Repository;
using KiCoKalender.Service;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Functions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Configuration;

namespace KiCoKalender.Startup {
	public class Program {
		public static void Main() {
			IHost host = new HostBuilder()
				.ConfigureFunctionsWorkerDefaults(worker => worker.UseNewtonsoftJson())
				.ConfigureServices(Configure)
				.Build();

			host.Run();
		}

		static void Configure(HostBuilderContext Builder, IServiceCollection Services) {
			Services.AddSingleton<IOpenApiHttpTriggerContext, OpenApiHttpTriggerContext>();
			Services.AddSingleton<IOpenApiTriggerFunction, OpenApiTriggerFunction>();
			
			Services.AddSingleton<IUserService, UserService>();
			Services.AddSingleton<UserRepository>();
			Services.AddSingleton<IAssetService, AssetService>();
			Services.AddSingleton<AssetRepository>();
			Services.AddSingleton<IAppointmentService, AppointmentService>();
			Services.AddSingleton<AppointmentRepository>();
		}
	}
}


