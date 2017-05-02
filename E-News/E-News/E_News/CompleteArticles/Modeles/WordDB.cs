namespace E_News
{
	public class WordDB
	{
		public int ArticleID { get; set; }

		public string Word { get; set; }

		public double TfIdfScore { get; set; }

        public int StringIndex { get; set; }
	}
}