using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace BlazorClock
{
	public class BlazorClockInterop
	{
		public static Task<bool> UpdateStyle(string id, string property, string value)
		{
			// Implemented in blazorClock.js
			return JSRuntime.Current.InvokeAsync<bool>(
					"blazorClock.setCssProperty", id, property, value);
		}
	}
}
