using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorTest.Server.Models
{
	public class ApplicationDataContext : IdentityDbContext<BlazorApplicationUser, IdentityRole<Guid>, Guid>
	{
		public ApplicationDataContext(DbContextOptions<ApplicationDataContext> options)
				: base(options)
		{ }

	}
}
