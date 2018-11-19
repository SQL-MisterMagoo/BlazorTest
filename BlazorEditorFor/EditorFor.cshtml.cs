using BlazorBoundComponent;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
using System;
using System.Linq;

namespace BlazorEditorFor
{
	public class EditorForModel<IType> : BlazorBoundComponent<IType>
	{

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
					switch (Data)
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
