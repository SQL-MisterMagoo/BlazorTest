using System.Security.Claims;

namespace BlazorSolidLogin.Services
{
	public class SolidIdentity : ILoginIdentity
	{
		private string _loginServer;
		public string LoginServer => _loginServer;

		private string _id;
		public string Id => _id;

		private string _name;
		public string Name => _name;

		ClaimsPrincipal _user;
		public ClaimsPrincipal User => _user;

		public SolidIdentity(string Server)
		{
			_loginServer = Server;
		}

		public void SetId(string Id)
		{
			_id = Id;
		}

		public void SetName(string Name)
		{
			_name = Name;
		}

		public void SetUser(ClaimsPrincipal User)
		{
			_user = User;
		}
	}
}