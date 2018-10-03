using System;
using BlazorTest.Server.Areas.Identity.Data;
using BlazorTest.Server.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(BlazorTest.Server.Areas.Identity.IdentityHostingStartup))]
namespace BlazorTest.Server.Areas.Identity
{
	public class IdentityHostingStartup : IHostingStartup
	{
		public void Configure(IWebHostBuilder builder)
		{
			builder.ConfigureServices((context, services) =>
			{
				services.AddDbContext<BlazorTestServerContext>(options =>
						options.UseSqlite(
								context.Configuration.GetConnectionString("BlazorTestServerContextConnection")));

				services.AddDefaultIdentity<BlazorTestServerUser>()
						.AddEntityFrameworkStores<BlazorTestServerContext>();
			});
		}
	}
}