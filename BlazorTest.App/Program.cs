using Microsoft.AspNetCore.Blazor.Hosting;

namespace BlazorTest.App
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IWebAssemblyHostBuilder CreateHostBuilder(string[] args) =>
				BlazorWebAssemblyHost.CreateDefaultBuilder()
						.UseBlazorStartup<Startup>();
	}
}
