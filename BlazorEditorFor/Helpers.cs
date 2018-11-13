namespace BlazorEditorFor
{
	public static class Helpers
	{
		public static string OrDefault(this string stringValue, string DefaultValue)
		{
			return string.IsNullOrWhiteSpace(stringValue) ? DefaultValue : stringValue;
		}
	}
}

