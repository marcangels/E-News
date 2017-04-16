using SQLite;
namespace E_News
{
	public class ArticleDB
	{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }

		public string Title { get; set; }

		public string Publisher { get; set; }

		public long Ticks { get; set; }

		public string Text { get; set; }

		public override string ToString()
		{
			return string.Format("[ArticleDB: ID={0}, Title={1}, Publisher={2}, Ticks={3}, Text={4}]", ID, Title, Publisher, Ticks, Text);
		}
	}
}