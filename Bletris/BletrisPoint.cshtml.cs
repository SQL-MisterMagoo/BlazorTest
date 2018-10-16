using Bletris.Model;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Bletris.Piece;

namespace Bletris
{
	public class BletrisPointModel : BlazorComponent
	{

		[Parameter] protected Point Point { get; set; }

		internal string Id;

		protected override void OnInit()
		{
			Id = $"BL{DateTime.Now.Ticks}";
		}

	}
}
