using BlazorBoundComponent;
using Microsoft.AspNetCore.Components;
using System;
using System.Linq;

namespace BlazorEditorFor
{
	public class EditorForModel<IType> : BlazorBoundComponent<IType>
	{

		[Parameter] public string InputType { get; set; }
		[Parameter] public string Id { get; set; }
		[Parameter] public string Class { get; set; }
		[Parameter] public string GroupClass { get; set; }
		[Parameter] public string InputClass { get; set; }
		[Parameter] public string LabelClass { get; set; }
		[Parameter] public string Style { get; set; }
		[Parameter] public string GroupStyle { get; set; }
		[Parameter] public string InputStyle { get; set; }
		[Parameter] public string LabelStyle { get; set; }
		[Parameter] public string Name { get; set; }

		protected override void OnInitialized()
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
						case bool _:
							return "checkbox";
						case string _:
							return "text";
						case int _:
						case double _:
						case long _:
						case short _:
							return "numeric";
						case DateTime _:
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
