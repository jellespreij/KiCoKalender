using Microsoft.Azure.Functions.Worker.Extensions.OpenApi;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Functions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
		}
	}
}


