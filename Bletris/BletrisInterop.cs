using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Bletris
{
	public class BletrisInterop
	{
		public static Task<bool> SetFocus(string id)
		{
			// Implemented in bletrisInterop.js
			return JSRuntime.Current.InvokeAsync<bool>(
								"bletrisInterop.setFocus",
								id);
		}
	}
}
