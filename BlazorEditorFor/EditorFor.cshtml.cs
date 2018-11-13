using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.RenderTree;
using System;
using System.Linq;

namespace BlazorEditorFor
{
	public class EditorForModel<IType> : BlazorComponent
	{

		[Parameter] protected IType For { get; set; }
		[Parameter] protected Action<IType> ForChanged { get; set; }
		[Parameter] protected string InputType { get; set; }
		[Parameter] protected string Id { get; set; }
		[Parameter] protected string Class { get; set; }
		[Parameter] protected string GroupClass { get; set; }
		[Parameter] protected string InputClass { get; set; }
		[Parameter] protected string LabelClass { get; set; }
		[Parameter] protected string Style { get; set; }
		[Parameter] protected string GroupStyle { get; set; }
		[Parameter] protected string InputStyle { get; set; }
		[Parameter] protected string LabelStyle { get; set; }
		[Parameter] protected string Name { get; set; }

		public IType For2;

		protected void OnForChanged(UIChangeEventArgs args)
		{
			For = (IType)Convert.ChangeType(args.Value, typeof(IType));
			For2 = For;
			ForChanged?.Invoke(For);
			StateHasChanged();
		}

		protected bool BindBool
		{
			get { return (bool)Convert.ChangeType(For, typeof(bool)); }
			set
			{
				For = (IType)Convert.ChangeType(value, typeof(IType));
				ForChanged?.Invoke(For);
				StateHasChanged();
			}
		}

		protected long BindLong
		{
			get { return (long)Convert.ChangeType(For, typeof(long)); }
			set
			{
				For = (IType)Convert.ChangeType(value, typeof(IType));
				ForChanged?.Invoke(For);
				StateHasChanged();
			}
		}

		protected override void OnInit()
		{
			InputType = InputType.OrDefault(GetInputType());
			Id = string.IsNullOrWhiteSpace(Id) ? Guid.NewGuid().ToString().Substring(0, 10) : Id;
			Name = string.IsNullOrWhiteSpace(Name) ? Id : Name;
			GroupClass = GroupClass.OrDefault(Class).OrDefault("form-group");
			InputClass = InputClass.OrDefault(Class).OrDefault("form-control");
			LabelClass = LabelClass.OrDefault(Class);
			GroupStyle = GroupStyle.OrDefault(Style);
			InputStyle = InputStyle.OrDefault(Style);
			LabelStyle = LabelStyle.OrDefault(Style);
		}

		private string GetInputType()
		{
			string fieldName = Name.Split('.').ToList().Last().ToLower();
			switch (fieldName)
			{
				case "password":
				case "email":
				case "url":
					return fieldName;
				default:
					switch (For)
					{
						case bool value:
							return "checkbox";
						case string value:
							return "text";
						case int valueInt:
						case double valueDouble:
						case long valueLong:
						case short valueShort:
							return "numeric";
						case DateTime valueDate:
							return "date";
						default:
							break;
					}
					break;
			}
			return "text";
		}


		//
	}
}
