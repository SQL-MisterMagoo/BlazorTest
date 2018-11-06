using System.Security.Claims;

namespace BlazorSolidLogin.Services
{
	public interface ILoginIdentity
	{
		string LoginServer { get; }
		string Id { get; }
		string Name { get;}
		ClaimsPrincipal User { get; }
	}
}