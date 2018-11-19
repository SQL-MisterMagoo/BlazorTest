using Microsoft.AspNetCore.Blazor.Components;
using System;

namespace BlazorBoundComponent
{
	public class BlazorBoundComponent<T> : BlazorComponent
	{
		// _data is the "actual" bound data
		internal T _data;

		// BoundData is a wrapper required by the runtime esp. when binding to <input> tags as they only like strings.
		public string BoundData
		{
			get
			{
				if (_data is DateTime)
					return String.Format("{0:yyyy-MM-dd}", _data);
				return _data.ToString();
			}
			set
			{
				try
				{
					Data = (T)Convert.ChangeType(value, typeof(T));
					DataChanged?.Invoke(Data);
				}
				catch { }
			}
		}

		// Data is the externally exposed param that the caller will bind to
		[Parameter] protected T Data { get { return _data; } set { _data = value; StateHasChanged(); } }

		// DataChanged is the externally exposes param that 
		// allows notifying the caller we changed their data.
		[Parameter] protected Action<T> DataChanged { get; set; }

	}
}
