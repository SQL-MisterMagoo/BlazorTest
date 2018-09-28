using Microsoft.AspNetCore.Blazor.Components;
using System;
using System.Threading.Tasks;

namespace BlazorClock.Model
{
	public class TickerModel : BlazorComponent
	{
		[Parameter] internal string Title { get; set; }
		[Parameter] internal double OffsetHours { get; set; }
		[Parameter] internal double OffsetMinutes { get; set; }
		[Parameter] internal string TimeZoneId { get; set; }
		[Parameter] internal bool AutoTitle { get; set; }
		[Parameter] internal bool ShowIcon { get; set; }
		[Parameter] internal double Width { get; set; }
		[Parameter] internal double Height { get; set; }

		internal string Data;
		internal double hourRotation;
		internal double minuteRotation;
		internal double secondRotation;

		private Task ticker;
		private TimeZoneInfo timeZone;

		protected override void OnInit()
		{
			if (!string.IsNullOrWhiteSpace(TimeZoneId))
			{
				try
				{
					timeZone = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId);
				}
				catch
				{
					timeZone = TimeZoneInfo.Local;
				}
			} else if (OffsetHours == 0 && OffsetMinutes == 0)
			{
				timeZone = TimeZoneInfo.Local;
			}  else
			{
				timeZone = TimeZoneInfo.CreateCustomTimeZone(Title ?? "Clock", new TimeSpan((int)OffsetHours, (int)OffsetMinutes, 0), Title ?? "Clock", Title ?? "Clock");
			}
			if (Width == 0) Width = 600;
			if (Height == 0) Height = 600;
			ticker = RunTicker();
		}

		private async Task RunTicker()
		{
			while (true)
			{
				await Task.Delay(1000);
				DateTime utcNow = DateTime.UtcNow;
				if (AutoTitle) Title = timeZone.IsDaylightSavingTime(utcNow) ? timeZone.DaylightName : timeZone.StandardName;
				DateTime dateTime = utcNow.Add(timeZone.GetUtcOffset(utcNow));
				Data = dateTime.ToLongTimeString();
				if (!ShowIcon) UpdateSvg(dateTime);
				StateHasChanged();
			}
		}

		private void UpdateSvg(DateTime date)
		{
			var hr = date.Hour;
			var min = date.Minute;
			var sec = date.Second;

			hourRotation = (hr * 360 / 12) + (min * (360 / 60) / 12);
			minuteRotation = (min * 360 / 60) + (sec * (360 / 60) / 60);
			secondRotation = sec * 360 / 60;
		}
	}
}
