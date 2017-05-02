using System;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
namespace E_News
{
	public static class Reader
	{
		public static List<Article> Articles(string source)
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://newsapi.org/v1/articles?source=" + source + "&sortBy=top&apiKey=c2400b26bb094f5d9de830a4b162642e");
			request.Method = WebRequestMethods.Http.Get;
			request.Accept = "application/json";
			var response = (HttpWebResponse)request.GetResponse();
			string text;
			using(var sr = new StreamReader(response.GetResponseStream()))
			{
				text = sr.ReadToEnd();
			}
			JavaScriptSerializer ser = new JavaScriptSerializer();
			var result = ser.Deserialize<Articles>(text);
			return result.articles;
		}

        public static List<GuardianArticle> GuardianArticles()
        {
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://content.guardianapis.com/search?api-key=bd9f7390-6744-42f0-9074-af4733475efd&show-fields=body&format=json");
			request.Method = WebRequestMethods.Http.Get;
			request.Accept = "application/json";
			var response = (HttpWebResponse)request.GetResponse();
			string text;
			using (var sr = new StreamReader(response.GetResponseStream()))
			{
				text = sr.ReadToEnd();
			}
			JavaScriptSerializer ser = new JavaScriptSerializer();
			var result = ser.Deserialize<GuardianObject>(text);
            return result.response.results;
        }

		public static string Content(string url)
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			request.Method = WebRequestMethods.Http.Get;
			var response = (HttpWebResponse)request.GetResponse();
			string text;
			using (var sr = new StreamReader(response.GetResponseStream()))
			{
				text = sr.ReadToEnd();
			}
			return text;
		}

		/*
        public static Errors Errors(string source)
        {
            string[] words = source.Split(' ');
            StringBuilder wordsBuilder = new StringBuilder();
            int l = words.Length;
            for (int i = 0; i < l - 1; i++)
            {
                wordsBuilder.Append(words[i]);
                wordsBuilder.Append("+");
            }
            wordsBuilder.Append(words[l-1]);
            string concatenated = wordsBuilder.ToString();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.textgears.com/check.php?text="+concatenated+"&key=Yb9hVD3rBo7XLaoV");
            request.Method = WebRequestMethods.Http.Get;
            request.Accept = "application/json";
            var response = (HttpWebResponse)request.GetResponse();
            string text;
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                text = sr.ReadToEnd();
            }
            JavaScriptSerializer ser = new JavaScriptSerializer();
            var result = ser.Deserialize<Errors>(text);
            return result;
        }
        */
	}
}