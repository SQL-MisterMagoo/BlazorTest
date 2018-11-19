using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
using System;

namespace BlazorBoundComponent
{
	public class BlazorBoundComponent<T> : BlazorComponent
	{
		// _data is the "actual" bound data
		internal T _data;

		// BoundData is a wrapper that allows us to control StateHasChanged
		public T BoundData
		{
			get
			{
				return _data;
			}
			set
			{
				try
				{
					//This takes care of the child data and refresh
					Data = value; 
					//This takes care of the parent data and refresh
					DataChanged?.Invoke(Data);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex);
				}
			}
		}

		// ChangeData can be useful for binding to onchange for things that require a string, like most <input> elements
		public void ChangeData(UIChangeEventArgs args)
		{
			try
			{
				if (_data is DateTime)
				{
					if (DateTime.TryParse(args.Value.ToString(), out DateTime dt))
						BoundData = (T)Convert.ChangeType(dt, typeof(T));
				}
				else
				{
					BoundData = (T)Convert.ChangeType(args.Value.ToString(), typeof(T));
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}

		// Data is the externally exposed param that the parent will bind to
		[Parameter] protected T Data { get { return _data; } set { _data = value; StateHasChanged(); } }

		// DataChanged is the externally exposes param that 
		// allows notifying the parent we changed their data.
		[Parameter] protected Action<T> DataChanged { get; set; }

	}
}
