using Microsoft.AspNetCore.Components;
using System;

namespace Bletris
{
    public class BletrisPointModel : ComponentBase
	{

		[Parameter] protected Point Point { get; set; }

		internal string Id;

		protected override void OnInit()
		{
			Id = $"BL{DateTime.Now.Ticks}";
		}

	}
}
