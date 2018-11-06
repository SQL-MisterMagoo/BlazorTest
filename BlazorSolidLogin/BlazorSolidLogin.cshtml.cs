using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorSolidLogin.Services;
using Microsoft.AspNetCore.Http;

namespace BlazorSolidLogin
{
	public class BlazorSolidLoginModel : BlazorComponent
	{
		[Inject] internal ILoginNotifier _session { get; set; }
		[Inject] IHttpContextAccessor httpContextAccessor { get; set; }

		internal string WebId { get; set; }
		internal string Name { get; set; }
		internal string LoginServer = "";

		protected override async Task OnInitAsync()
		{
			Console.WriteLine("Login Init");
			_session.LoginStateChanged += LoginChanged;
			await _session.GetLoginSession();
		}

		private void LoginChanged(bool LoggedIn)
		{
			Console.WriteLine($"LoginChanged: {LoggedIn}");
			WebId = LoggedIn ? _session?.Identity.Id : null;
			Name = LoggedIn ? $"[{_session?.Identity.Name}]" : "";
			StateHasChanged();
		}

		protected async Task LoginClick(UIMouseEventArgs args)
		{
			Console.WriteLine("Login Clicked");
			HttpRequest request = httpContextAccessor.HttpContext.Request;
			string callbackUrl = $"{request.Headers["Origin"]}{request.PathBase}/";
			await _session?.InitiateLogin(LoginServer, callbackUrl);
		}

		protected async Task LogoutClick(UIMouseEventArgs args)
		{
			await JSRuntime.Current.InvokeAsync<object>("solid.auth.logout", new object[] {  });
			_session?.UserLoggedOut();
		}
	}
}

