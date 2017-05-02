using System;
using System.Text.RegularExpressions;
using System.Net;
using System.Text;
namespace E_News
{
	public static class Parser
	{
        /*
        private static string IgnoreOpeningAnchors(string text)
        {
            return Regex.Replace(text, "<a .*?>", String.Empty);
        }

        private static string IgnoreClosingAnchors(string text)
        {
            return Regex.Replace(text, "</a>", String.Empty);
        }

        public static string IgnoreAnchors(string text)
        {
            return IgnoreClosingAnchors(IgnoreOpeningAnchors(text));
        }
        */


		public static string IgnoreHTML(string text)
		{
            text = Regex.Replace(text, "<figure .*>.*</figure>", String.Empty);
			text = Regex.Replace(text, "<p>", "\n\n");
			text = Regex.Replace(text, "<.*?>", String.Empty);
            return text;
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

        public static string ParseGuardian(string text)
        {
            // TODO : probably done.
            // lignes debut + fin d'article -> vide
            // saut de lignes -> 1 ligne
            // alinea -> vide
            text = Regex.Replace(text, @"\s?<aside .*> <p> <span>Related:.*</p> </aside>\s?", String.Empty);
            text = IgnoreHTML(text);
            text = Regex.Replace(text, "(\n){3,}", "\n\n");
            text = text.Trim();
            return text;
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