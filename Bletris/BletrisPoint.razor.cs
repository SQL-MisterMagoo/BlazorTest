using Microsoft.AspNetCore.Components;
using System;

namespace Bletris
{
    public class BletrisPointModel : ComponentBase
	{

		[Parameter] public Point Point { get; set; }

		internal string Id;

		protected override void OnInitialized()
		{
			Id = $"BL{DateTime.Now.Ticks}";
		}

	}
}
