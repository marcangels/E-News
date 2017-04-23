using System;
using System.Text.RegularExpressions;
using System.Net;
namespace E_News
{
	public static class Parser
	{
		public static string IgnoreHTML(string text)
		{
			return Regex.Replace(text, "<.*?>", String.Empty);
		}

		public static string IgnoreCSS(string text)
		{
			return Regex.Replace(text, "<style>(.*|\\n)</style>", String.Empty, RegexOptions.Multiline);
		}

		public static string IgnoreJavaScript(string text)
		{
			return Regex.Replace(text, "<script.*?>(.*|\\n)</script>", String.Empty, RegexOptions.Multiline);
		}

		public static string DecodeHTML(string text)
		{
			return WebUtility.HtmlDecode(text);
		}

		public static string DecodeUnicode(string text)
		{

			return Regex.Unescape(text);
		}

		public static string Parse(string content, string publisher)
		{
			switch (publisher)
			{
				case "techcrunch":
					return DecodeUnicode(DecodeHTML(ParseTechCrunch(content)));
				case "ars-technica":
					return DecodeUnicode(DecodeHTML(ParseArsTechnica(content)));
				default:
					return null;
			}
		}

		public static string ParseTechCrunch(string content)
		{
			var startRegex = "\"articleBody\":\"";
			var endRegex = "\",\"datePublished\":\"";
			var i1 = content.IndexOf(startRegex, System.StringComparison.Ordinal);
			var i2 = content.LastIndexOf(endRegex, System.StringComparison.Ordinal);
			i1 = i1 + startRegex.Length;
			var text = content.Substring(i1, i2 - i1);
			return text;
		}

		public static string ParseArsTechnica(string content)
		{
			var startRegex = "<p>";
			var endRegex = "</p>";
			var pCount = Utility.Count(content, endRegex);
			var i1 = Utility.IndexOfNth(content, startRegex, 4);
			var i2 = Utility.IndexOfNth(content, endRegex, pCount - 1);
			i1 = i1 + startRegex.Length;
			var text = content.Substring(i1, i2 - i1);
			text = Regex.Replace(text, "Listing image.*", String.Empty, RegexOptions.Singleline);
			text = Regex.Replace(text, ".*@[a-zA-Z0-9].*", String.Empty, RegexOptions.Multiline);
			text = IgnoreHTML(text);
			return text;
		}
	}
}