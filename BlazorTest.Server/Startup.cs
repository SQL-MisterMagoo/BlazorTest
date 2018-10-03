using BlazorTest.Server.Areas.Identity.Data;
using BlazorTest.Server.Models;
using Microsoft.AspNetCore.Blazor.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

namespace BlazorTest.Server
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			// Adds the Server-Side Blazor services, and those registered by the app project's startup.
			services.AddServerSideBlazor<App.Startup>();

			services.AddResponseCompression(options =>
			{
				options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
							{
										MediaTypeNames.Application.Octet,
										WasmMediaTypeNames.Application.Wasm,
					});
			});

			services.Configure<CookiePolicyOptions>(options =>
			{
				// This lambda determines whether user consent for non-essential cookies is needed for a given request.
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});
			
			services.AddAuthentication().AddMicrosoftAccount(microsoftOptions =>
			{
				microsoftOptions.ClientId = Configuration["Authentication:Microsoft:ApplicationId"];
				microsoftOptions.ClientSecret = Configuration["Authentication:Microsoft:Password"];
				microsoftOptions.CallbackPath = "/signin-microsoft";
			});
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			app.UseResponseCompression();

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseCookiePolicy();

			app.UseAuthentication();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
									name: "default",
									template: "{controller=Home}/{action=Index}/{id?}");
			});
			// Use component registrations and static files from the app project.
			app.Map("/blz", (child) => child.UseServerSideBlazor<App.Startup>());
		}
	}
}
