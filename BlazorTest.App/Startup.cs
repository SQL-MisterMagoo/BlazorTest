using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorTestApp
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddScoped<UserManager<BlazorTestServerUser>, UserManager<BlazorTestServerUser>>();
            //services.AddScoped<SignInManager<BlazorTestServerUser>, SignInManager<BlazorTestServerUser>>();
            //services.AddTransient<ILoginNotifier>((a) =>
            //{
            //    var JSRuntime = a.GetRequiredService<IJSRuntime>();
            //    return new SolidIdentityService(JSRuntime);
            //});

            //services.ToList().ForEach(s => Console.WriteLine($"Service: {s.ServiceType.Name} - {s.Lifetime.ToString()}"));

        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
