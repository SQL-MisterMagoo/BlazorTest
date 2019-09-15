using Microsoft.JSInterop;
using Newtonsoft.Json.Linq;
using System;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSolidLogin.Services
{
	public class SolidIdentityService : ILoginNotifier
	{
		public Action<bool> LoginStateChanged { get; set; }
		public bool IsLoggedIn => solidIdentity != null;

		private SolidIdentity solidIdentity;
		ILoginIdentity ILoginNotifier.Identity => solidIdentity;

        public IJSRuntime JSRuntime { get; }

        public SolidIdentityService(IJSRuntime runtime)
		{
            JSRuntime = runtime;
		}

		ValueTask UserLoggedIn()
		{
			return GetLoginSession();
		}

		ValueTask UserLoggedOut()
		{
			solidIdentity = null;
			LoginStateChanged?.Invoke(false);
			return Task.CompletedTask;
		}

		public async Task GetLoginSession()
		{
			solidIdentity = null;
			try
			{
				var result = await JSRuntime.InvokeAsync<object>("solid.auth.currentSession", null);
				Console.WriteLine($"Result: {result ?? ""}");

				if (result != null)
				{
					JObject jObject = JObject.Parse(result.ToString());

					solidIdentity = new SolidIdentity((string)jObject["issuer"]);
					solidIdentity.SetId((string)jObject["webId"]);

					string name = null;
					try
					{
						var nameResult = await JSRuntime.InvokeAsync<object>("blazorSolid.getUserName", solidIdentity.Id);
						if (nameResult != null)
						{
							JObject nameObject = JObject.Parse(nameResult.ToString());

							name = (string)nameObject["value"];
						}
					}
					catch (Exception x2)
					{
						Console.WriteLine("NAME ERROR:" + x2.GetBaseException().Message);
					}

					if (!string.IsNullOrWhiteSpace(name))
					{
						solidIdentity.SetName(name);
					}
					else
					{
						Uri uri = new Uri(solidIdentity.Id);
						solidIdentity.SetName(uri.DnsSafeHost.Split('.')[0]);
					}
				}
			}
			catch { }
			if (solidIdentity == null)
			{
				LoginStateChanged?.Invoke(false);
			}
			else
			{
				LoginStateChanged?.Invoke(true);
			}
		}

		public async Task InitiateLogin(string loginServer, string callbackUrl)
		{
			Options options = new Options { CallbackUri = callbackUrl };
			var result = await JSRuntime.InvokeAsync<object>("solid.auth.login", new object[] { loginServer, options });
		}

	}

	public class Options
	{
		public string CallbackUri { get; set; }
	}

}
