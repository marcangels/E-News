using SQLite;
namespace E_News
{
	public class PublisherDB
	{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }

		[Unique]
		public string Name { get; set; }
	}
}