using System;
using System.Threading.Tasks;

namespace BlazorSolidLogin.Services
{
	public interface ILoginNotifier
	{
		ILoginIdentity Identity { get; }
		bool IsLoggedIn { get; }
		Action<bool> LoginStateChanged { get; set; }
		Task UserLoggedIn();		
		Task UserLoggedOut();
		Task GetLoginSession();
		Task InitiateLogin(string loginServer, string callbackUrl);
	}
}
