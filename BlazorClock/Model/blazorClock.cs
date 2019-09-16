using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace BlazorClock
{
	public static class BlazorClockInterop
	{
		public static ValueTask<bool> UpdateStyle(this IJSRuntime JSRuntime, string id, string property, string value)
		{
			// Implemented in blazorClock.js
			return JSRuntime.InvokeAsync<bool>(
					"blazorClock.setCssProperty", id, property, value);
		}
	}
}
