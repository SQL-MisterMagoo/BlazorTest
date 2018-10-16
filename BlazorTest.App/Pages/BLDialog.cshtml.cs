using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace BlazorTest.App.Pages
{
	public class BLDialogModel : BlazorComponent
	{
		Timer timer;

		public bool ShowDialog { get; private set; }

		protected override void OnInit()
		{
			base.OnInit();
			timer = new Timer(2000);
			timer.Elapsed += AnnoyPeople;
			timer.Start();
		}

		private void AnnoyPeople(object sender, ElapsedEventArgs e)
		{
			timer.Stop();
			ShowDialog = true;
			StateHasChanged();
		}

		internal void OnClick(UIMouseEventArgs args)
		{
			timer.Start();
			ShowDialog = false;
			StateHasChanged();
		}
	}
}
