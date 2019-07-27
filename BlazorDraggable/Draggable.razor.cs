using Microsoft.AspNetCore.Components;
using System;

namespace BlazorDraggable
{

    public class DraggableModel : ComponentBase
	{

		[Parameter] protected RenderFragment<object> ChildContent { get; set; }
		[Parameter] protected string Class { get; set; }
		[Parameter] protected object Data { get; set; }
		[Parameter] protected Action<object> DragStarted { get; set; }
		[Parameter] protected Action<object> DragEnter { get; set; }
		[Parameter] protected Action<object> DragLeave { get; set; }
		[Parameter] protected Action DragEnd { get; set; }

		public long ClientX { get; private set; }
		public long ClientY { get; private set; }
		public bool isUnderDragItem { get; private set; }
		public bool isBeingDragged { get; private set; }

		protected void OnDragStart(UIDragEventArgs args)
		{
			Console.WriteLine($"Start {Data}");
			isBeingDragged = true;
			args.DataTransfer.EffectAllowed = "move";
			args.DataTransfer.Types = new string[] { "text/plain" };
			args.DataTransfer.Items = new UIDataTransferItem[] { new UIDataTransferItem() { Kind = "string", Type = "text/plain" } };
			ClientX = args.ClientX;
			ClientY = args.ClientY;
			DragStarted?.Invoke(Data);
		}

		protected void OnDragEnd(UIDragEventArgs args)
		{
			Console.WriteLine($"End");
			isBeingDragged = false;
			isUnderDragItem = false;
			DragEnd?.Invoke();
		}

		protected void OnDragEnter(UIDragEventArgs args)
		{
			Console.WriteLine($"Over {Data}");
			args.DataTransfer.DropEffect = "move";
			isUnderDragItem = true;
			DragEnter?.Invoke(Data);
		}

		protected void OnDragLeave(UIDragEventArgs args)
		{
			Console.WriteLine($"Leave {Data}");
			isUnderDragItem = false;
			DragLeave?.Invoke(Data);
		}

		protected void OnDrop(UIDragEventArgs args)
		{
			Console.WriteLine($"Drop {Data}");
			isBeingDragged = false;
			isUnderDragItem = false;
			DragEnter?.Invoke(Data);
		}

		public string Styles()
		{
			return "";
		}
	}
}
