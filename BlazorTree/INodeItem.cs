using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorTree
{
	public interface INodeItem<T>
	{
		IEnumerable<T> Dependents { get; set; }
		int Depth { get; set; }
		NodeState State { get; set; }
	}
	public enum NodeState
	{
		Collapsed,
		Expanded
	}
}
