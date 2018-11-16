using System.Collections.Generic;
using System.Linq;

namespace BlazorTree
{
	public class TreeSelection<T> : ITreeSelection<T>
	{
		List<T> SelectedItems;

		public TreeSelection()
		{
			SelectedItems = new List<T>();
		}

		public void Clear()
		{
			SelectedItems.Clear();
		}

		public IEnumerable<T> GetSelected()
		{
			return SelectedItems?.AsEnumerable<T>();
		}

		public bool IsSelected(T Item)
		{
			return SelectedItems?.Contains(Item) ?? false;
		}

		public void Select(T Item, bool ClearOtherSelections = false)
		{
			if (IsSelected(Item))
			{
				SelectedItems.Remove(Item);
				return;
			}
			if (ClearOtherSelections)
			{
				Clear();
			}
			SelectedItems?.Add(Item);
		}
	}
}
