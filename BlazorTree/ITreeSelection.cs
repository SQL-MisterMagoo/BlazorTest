using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorTree
{
	/// <summary>
	/// Interface ITreeSelection can be implemented by the consumer and made available through CascadeValue to the <see cref="BlazorTree">BlazorTree</see>
	/// </summary>
	/// <typeparam name="T">Generic Type that matches the Type of data passed to the tree to display.</typeparam>
	public interface ITreeSelection<T>
	{
		/// <summary>
		/// Implement this method so that the Tree nodes can determine if they have been selected.
		/// </summary>
		/// <param name="Item">The tree node will pass in an Item of Type T</param>
		/// <returns></returns>
		bool IsSelected(T Item);
		/// <summary>
		/// Returns a list of all selected Nodes.
		/// </summary>
		/// <returns>IEnumerable of T</returns>
		IEnumerable<T> GetSelected();
		/// <summary>
		/// Implement this method to allow nodes to be selected.
		/// </summary>
		/// <param name="Item">The Item of Type T to select.</param>
		/// <param name="ClearOtherSelections">Set true to make this the only selected node.</param>
		void Select(T Item, bool ClearOtherSelections);
		/// <summary>
		/// Clear all selections.
		/// </summary>
		void Clear();
	}
}
