using Microsoft.AspNetCore.Components;
using System;
using System.Text.RegularExpressions;

namespace BlazorTestApp.Extensions
{
    public static class MyExtensions
	{
		public static void NavigateTo<T>(this IUriHelper uriHelper, string childPath="")
		{
			NavigationManager.NavigateTo(typeof(T), childPath);
		}

		public static void NavigateTo(this IUriHelper uriHelper, Type page, string childPath="")
		{
			NavigationManager.NavigateTo($"{page.Name.ToSlug()}/{childPath?.ToSlug()}".TrimEnd('/'));
		}

		/// <summary>
		/// Code from https://stackoverflow.com/a/2921135/2658697
		/// </summary>
		/// <param name="phrase">a string to Slugify</param>
		/// <returns></returns>
		public static string ToSlug(this string phrase)
		{
			string str = phrase.RemoveAccent().ToLower();
			// invalid chars           
			str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
			// convert multiple spaces into one space   
			str = Regex.Replace(str, @"\s+", " ").Trim();
			// cut and trim 
			str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
			str = Regex.Replace(str, @"\s", "-"); // hyphens   
			return str;
		}

		public static string RemoveAccent(this string txt)
		{
			byte[] bytes = System.Text.Encoding.GetEncoding("Unicode").GetBytes(txt);
			return System.Text.Encoding.ASCII.GetString(bytes);
		}
	}
}
