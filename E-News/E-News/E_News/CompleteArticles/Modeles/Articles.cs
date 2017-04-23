using System.Collections.Generic;
namespace E_News
{
	public class Articles
	{
		public string status { get; set; }
		public string source { get; set; }
		public string sortBy { get; set; }
		public List<Article> articles { get; set; }
	}
}