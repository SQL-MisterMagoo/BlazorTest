using BlazorSolidLogin.Services;
using Microsoft.AspNetCore.Blazor.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace BlazorTest.App
{
	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddTransient<ILoginNotifier>((a)=> new SolidIdentityService());
			
			//services.ToList().ForEach(s => Console.WriteLine($"Service: {s.ServiceType.Name} - {s.Lifetime.ToString()}"));

		}

		public void Configure(IBlazorApplicationBuilder app)
		{
			app.AddComponent<App>("app");
			
		}
	}
}
