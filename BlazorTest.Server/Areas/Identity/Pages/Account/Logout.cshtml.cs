﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using BlazorTest.Server.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace BlazorTest.Server.Areas.Identity.Pages.Account
{
	[AllowAnonymous]
	public class LogoutModel : PageModel
	{
		private readonly SignInManager<BlazorTestServerUser> _signInManager;
		private readonly ILogger<LogoutModel> _logger;

		public LogoutModel(SignInManager<BlazorTestServerUser> signInManager, ILogger<LogoutModel> logger)
		{
			_signInManager = signInManager;
			_logger = logger;
		}

		public async Task<IActionResult> OnGet(string returnUrl = null)
		{
			await _signInManager.SignOutAsync();
			_logger.LogInformation("User logged out.");
			if (returnUrl != null)
			{
				return LocalRedirect(returnUrl);
			}
			else
			{
				return LocalRedirect("/blz");
			}
		}

		public async Task<IActionResult> OnPost(string returnUrl = null)
		{
			await _signInManager.SignOutAsync();
			_logger.LogInformation("User logged out.");
			if (returnUrl != null)
			{
				return LocalRedirect(returnUrl);
			}
			else
			{
				return Page();
			}
		}
	}
}