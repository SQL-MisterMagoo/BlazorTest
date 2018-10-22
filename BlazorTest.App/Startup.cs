using Microsoft.AspNetCore.Blazor.Builder;
using Microsoft.Extensions.DependencyInjection;
using BlazorTest.App.Services;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace BlazorTest.App
{
	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
			// Since Blazor is running on the server, we can use an application service
			// to read the forecast data.
			var logger = services.BuildServiceProvider().GetRequiredService<ILogger<string>>();
			services.ToList().ForEach(s => logger.LogInformation($"Service: {s.ServiceType.Name} - {s.Lifetime.ToString()}"));

		}

		public void Configure(IBlazorApplicationBuilder app)
		{
			app.AddComponent<App>("app");
			
		}
	}
}
