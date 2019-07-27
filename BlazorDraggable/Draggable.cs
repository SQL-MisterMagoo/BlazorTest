using Microsoft.AspNetCore.Components;
using System;

namespace BlazorDraggable
{
    public class Draggable<TItem> : ComponentBase
    {
        /// <summary>
        /// html element id - defaults to pseudo random to prevent diff problems
        /// </summary>
        [Parameter] protected string ID { get; set; } = Math.Abs(Guid.NewGuid().GetHashCode()).ToString();
        /// <summary>
        /// place your markup here
        /// </summary>
        [Parameter] protected RenderFragment<TItem> DragContent { get; set; }
        /// <summary>
        /// A data item to identify the thing being dragged and for rendering the DragContent
        /// </summary>
        [Parameter] protected TItem DataItem { get; set; }
        /// <summary>
        /// The item currently being dragged
        /// </summary>
        [Parameter] protected TItem DragItem { get; set; }
        /// <summary>
        /// HTML5 drag type allowed for this item - required - default is "move"
        /// </summary>
        [Parameter] protected string DragType { get; set; } = "move";
        /// <summary>
        /// What type of drop to allow - default to "move"
        ///</summary>
        [Parameter] protected string DropType { get; set; } = "move";
        /// <summary>
        /// CSS Class added while another item is being dragged
        /// </summary>
        [Parameter] protected string IdleClass { get; set; } = " draggable-idle";
        /// <summary>
        /// CSS class for the draggable item - optional
        /// </summary>
        [Parameter] protected string DraggableClass { get; set; }
        /// <summary>
        /// Should you need to apply an inline style, I'm not going to stop you
        /// </summary>
        [Parameter] protected string DraggableStyle { get; set; }
        /// <summary>
        /// Flag that identifies if this item is the thing being dragged
        /// </summary>
        [Parameter] protected bool IsDragItem { get; set; }
        /// <summary>
        /// CSS class to apply when IsDragItem is true
        /// </summary>
        [Parameter] protected string DragItemClass { get; set; }
        /// <summary>
        /// Flag to enable console logging
        /// </summary>
        [Parameter] protected bool Debug { get; set; }
        /// <summary>
        /// Flag to indicate if this is the active dropzone
        ///</summary>
        [Parameter] protected bool IsDropTarget { get; set; }
        /// <summary>
        /// CSS class to use when this is the active dropzone
        ///</summary>
        [Parameter] protected string DropTargetClass { get; set; }

        [Parameter] protected Action<UIDragEventArgs, TItem> OnDragStart { get; set; }
        [Parameter] protected Action<UIDragEventArgs, TItem> OnDragEnd { get; set; }
        [Parameter] protected Action<UIDragEventArgs, TItem> OnDragEnter { get; set; }
        [Parameter] protected Action<UIDragEventArgs, TItem> OnDragDrop { get; set; }
        [Parameter] protected Action<UIDragEventArgs, TItem> OnDragLeave { get; set; }
        [Parameter] protected Action<UIDragEventArgs, TItem> OnDragOver { get; set; }

        string ClassList => (!IsDragItem ? (DraggableClass ?? "") : DragItemClass ?? "") + ((IsDropTarget && (DragItem is object)) ? DropTargetClass ?? "" : "");

        void MyDragStart(UIDragEventArgs args)
        {
            if (Debug) Console.WriteLine($"DR: {DataItem} START");
            args.DataTransfer.EffectAllowed = DragType;
            args.DataTransfer.Types = new string[] { "text/plain" };
            args.DataTransfer.Items = new UIDataTransferItem[] { new UIDataTransferItem() { Kind = "string", Type = "text/plain" } };
            OnDragStart?.Invoke(args, DataItem);
        }
        void MyDragEnd(UIDragEventArgs args)
        {
            if (Debug) Console.WriteLine($"DR: {DataItem} END");
            OnDragEnd?.Invoke(args, DataItem);
        }
        string DragStartJS => $"event.dataTransfer.effectAllowed = '{DragType}'; event.dataTransfer.setData('text/plain', event.target.id);";
        string DragDropJS => "if (event.preventDefault) event.preventDefault(); if (event.stopPropagation) event.stopPropagation();";
        string DragOverJS => $"if (event.preventDefault) {{ event.preventDefault(); }}; event.dataTransfer.dropEffect = '{DropType}';";

        void MyDragEnter(UIDragEventArgs args)
        {
            if (Debug) Console.WriteLine($"DZ:{DataItem} ENTER");
            IsDropTarget = true;
            OnDragEnter?.Invoke(args, DataItem);
        }

        void MyDragLeave(UIDragEventArgs args)
        {
            if (Debug) Console.WriteLine($"DZ:{DataItem} LEAVE");
            IsDropTarget = false;
            OnDragLeave?.Invoke(args, DataItem);
        }

        void MyDragDrop(UIDragEventArgs args)
        {
            if (Debug) Console.WriteLine($"DZ:{DataItem} DROP");
            IsDropTarget = false;
            OnDragDrop?.Invoke(args, DataItem);
        }

        void MyDragOver(UIDragEventArgs args)
        {
            if (Debug) Console.WriteLine($"DZ:{DataItem} OVER");
            IsDropTarget = true;
            OnDragOver?.Invoke(args, DataItem);
        }

        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.RenderTree.RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            int c = 0;

            builder.OpenElement(c++, "drag-item");
            builder.AddAttribute(c++, "id", ID);
            builder.AddAttribute(c++, "draggable", "true");
            builder.AddAttribute(c++, "ondragover", DragOverJS);
            builder.AddAttribute(c++, "ondragstart", DragStartJS);
            builder.AddAttribute(c++, "ondrop", DragDropJS);
            builder.AddAttribute(c++, "ondragleave", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.UIDragEventArgs>(this, MyDragLeave));
            builder.AddAttribute(c++, "ondrop", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.UIDragEventArgs>(this, MyDragDrop));
            builder.AddAttribute(c++, "ondragenter", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.UIDragEventArgs>(this, MyDragEnter));
            builder.AddAttribute(c++, "ondragstart", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.UIDragEventArgs>(this, MyDragStart));
            builder.AddAttribute(c++, "ondragend", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.UIDragEventArgs>(this, MyDragEnd));
            if (!(OnDragOver is null))
            {
                c = 20; //Ensure the attribute always has the same sequence
                builder.AddAttribute(c++, "ondragover", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.UIDragEventArgs>(this, MyDragOver));
            }
            if (!string.IsNullOrWhiteSpace(ClassList))
            {
                c = 30; //Ensure the attribute always has the same sequence
                builder.AddAttribute(c++, "class", ClassList);
            }
            if (!string.IsNullOrWhiteSpace(DraggableStyle))
            {
                c = 40; //Ensure the attribute always has the same sequence
                builder.AddAttribute(c++, "style", DraggableStyle);
            }
            c = 98; //Ensure the closing content always has the same sequence
            builder.AddContent(99, DragContent(DataItem));
            builder.CloseElement();
        }

    }
}