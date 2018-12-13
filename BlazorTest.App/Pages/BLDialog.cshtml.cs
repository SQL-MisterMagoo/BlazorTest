using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.Layouts;
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
		[CascadingParameter(Name ="GlobalDialog")] dynamic MainLayout { get; set; }

		Timer timer;

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
			timer.Elapsed -= AnnoyPeople;
			MainLayout?.ShowDialog(true);
			timer.Elapsed += OnClick;
			timer.Start();
		}

		internal void OnClick(object sender, ElapsedEventArgs e)
		{
			timer.Stop();
			timer.Elapsed -= OnClick;
			MainLayout?.ShowDialog(false);
			timer.Elapsed += AnnoyPeople;
			timer.Start();
		}
	}
}
