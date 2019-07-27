using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorClock
{
	public class ClockModel : ComponentBase
	{
		[Parameter] internal string Title { get; set; }
		[Parameter] internal double OffsetHours { get; set; }
		[Parameter] internal double OffsetMinutes { get; set; }
		[Parameter] internal string TimeZoneId { get; set; }
		[Parameter] internal bool AutoTitle { get; set; }
		[Parameter] internal bool ShowIcon { get; set; }
		[Parameter] internal double Width { get; set; }
		[Parameter] internal double Height { get; set; }
		[Parameter] internal string ClockId { get; set; }
		[Parameter] protected bool AlwaysActive { get; set; }

		[Inject] private IUriHelper UriHelper { get; set; }
        [Inject] private IJSRuntime JSRuntime { get; set; }

		internal string Data;
		internal double hourRotation;
		internal double minuteRotation;
		internal double secondRotation;
		private CancellationTokenSource TokenSource;
		private Task ClockTask;
		private TimeZoneInfo TimeZone;
		private string ThisURI;
		private bool ShouldBeActiveNow;

		protected override void OnInit()
		{
			ShouldBeActiveNow = true;
			ThisURI = UriHelper.GetAbsoluteUri();
			UriHelper.OnLocationChanged += UriHelper_OnLocationChanged;

			if (!string.IsNullOrWhiteSpace(TimeZoneId))
			{
				try
				{
					TimeZone = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId);
				}
				catch
				{
					TimeZone = TimeZoneInfo.Local;
				}
			}
			else if (OffsetHours == 0 && OffsetMinutes == 0)
			{
				TimeZone = TimeZoneInfo.Local;
			}
			else
			{
				TimeZone = TimeZoneInfo.CreateCustomTimeZone(Title ?? "Clock", new TimeSpan((int)OffsetHours, (int)OffsetMinutes, 0), Title ?? "Clock", Title ?? "Clock");
			}
			if (Width == 0) Width = 50;
			if (Height == 0) Height = 50;
			if (string.IsNullOrWhiteSpace(ClockId)) ClockId = $"C{Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10)}";
			TokenSource = new CancellationTokenSource();
			ClockTask = RunClock(TokenSource.Token);
		}

		private void UriHelper_OnLocationChanged(object sender, LocationChangedEventArgs e)
		{
			if (AlwaysActive)
			{
				return;
			}
			ShouldBeActiveNow = ShouldMatch(UriHelper.GetAbsoluteUri());
			if (ShouldBeActiveNow)
			{				
				ClockTask = RunClock(TokenSource.Token);
			}
			else
			{
				TokenSource.Cancel();
			}
		}

		private bool ShouldMatch(string currentUriAbsolute)
		{
			if (EqualsHrefExactlyOrIfTrailingSlashAdded(currentUriAbsolute))
			{
				return true;
			}

			return false;
		}

		private bool EqualsHrefExactlyOrIfTrailingSlashAdded(string currentUriAbsolute)
		{
			if (string.Equals(currentUriAbsolute, ThisURI, StringComparison.Ordinal))
			{
				return true;
			}

			if (currentUriAbsolute.Length == ThisURI.Length - 1)
			{
				// Special case: highlight links to http://host/path/ even if you're
				// at http://host/path (with no trailing slash)
				//
				// This is because the router accepts an absolute URI value of "same
				// as base URI but without trailing slash" as equivalent to "base URI",
				// which in turn is because it's common for servers to return the same page
				// for http://host/vdir as they do for host://host/vdir/ as it's no
				// good to display a blank page in that case.
				if (ThisURI[ThisURI.Length - 1] == '/'
						&& ThisURI.StartsWith(currentUriAbsolute, StringComparison.Ordinal))
				{
					return true;
				}
			}
			
			return false;
		}

		private async Task RunClock(CancellationToken cancellationToken)
		{
			await Task.Delay(40);
			while (!cancellationToken.IsCancellationRequested)
			{
				DateTime utcNow = DateTime.UtcNow;
				if (AutoTitle) Title = TimeZone.IsDaylightSavingTime(utcNow) ? TimeZone.DaylightName : TimeZone.StandardName;
				DateTime dateTime = utcNow.Add(TimeZone.GetUtcOffset(utcNow));
				Data = dateTime.ToLongTimeString();
				if (!ShowIcon) UpdateSvg(dateTime);
				StateHasChanged();
				await Task.Delay(1000);
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
			JSRuntime.UpdateStyle($"#{ClockId} #second", "--rotation", $"{secondRotation}deg");
			JSRuntime.UpdateStyle($"#{ClockId} #minute", "--rotation", $"{minuteRotation}deg");
			JSRuntime.UpdateStyle($"#{ClockId} #hour", "--rotation", $"{hourRotation}deg");
		}
	}
}
