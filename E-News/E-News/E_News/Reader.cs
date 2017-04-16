using System.Net;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
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
			//JavaScriptSerializer ser = new JavaScriptSerializer();
			//var result = ser.Deserialize<Articles>(text);
			var result = JsonConvert.DeserializeObject<Articles>(text);
			return result.articles;
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
	}
}