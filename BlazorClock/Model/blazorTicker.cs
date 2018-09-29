using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace BlazorClock
{
	public class blazorTicker
	{
		public static Task<bool> UpdateStyle(string id, string property, string value)
		{
			// Implemented in blazorTicker.js
			return JSRuntime.Current.InvokeAsync<bool>(
					"blazorTicker.setCssProperty", id, property, value);
		}
	}
}
