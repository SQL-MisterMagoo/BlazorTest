using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BlazorTraffic
{
	public class TrafficModel : BlazorComponent
    {
		public struct TrafficDataStruct
		{
			public long Bytes;
			public long BytesPerSecond;
		}

		internal TrafficDataStruct TrafficData;

		private Task monitor;

		[Parameter] protected string CssClass { get; set; }
		[Parameter] protected string DisplayFormatString { get; set; }
		[Parameter] protected RenderFragment<TrafficDataStruct> Parts { get; set; }

		[Inject] IHttpContextAccessor httpContextAccessor { get; set; }

		protected override void OnInit()
		{
			monitor = MonitorTraffic();
			CssClass = CssClass ?? "";
			DisplayFormatString = DisplayFormatString ?? "{0:N}";
		}

		private async Task MonitorTraffic()
		{
			long previous = 0;
			await Task.Delay(40);
			while (true)
			{

				try
				{
					TrafficData.Bytes = ((dynamic)httpContextAccessor.HttpContext.Request).HttpContext.Features.ConnectionFeatures.TotalBytesWritten;
					TrafficData.BytesPerSecond = TrafficData.Bytes - previous;
					previous = TrafficData.Bytes;
				}
				catch { }

				StateHasChanged();
				await Task.Delay(1000);
			}
		}

	}
}
