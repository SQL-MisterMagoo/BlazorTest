﻿using Microsoft.AspNetCore.Components;

using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BlazorTraffic
{
	public class TrafficModel : ComponentBase
    {
		public struct TrafficDataStruct
		{
			public long Bytes;
			public long BytesPerSecond;
		}

		internal TrafficDataStruct TrafficData;

		private Task monitor;

		[Parameter] public string CssClass { get; set; }
		[Parameter] public string DisplayFormatString { get; set; }
		[Parameter] public RenderFragment<TrafficDataStruct> Parts { get; set; }

		[Inject] IHttpContextAccessor httpContextAccessor { get; set; }

		protected override void OnInitialized()
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
