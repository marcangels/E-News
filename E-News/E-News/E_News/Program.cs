using System;
using SQLite;
namespace E_News
{
	class MainClass
	{
		public static void Main()
		{
			/*
			string publisher = Utility.PUBLISHERS[0]; // techcrunch
			List<Article> articles = Reader.Articles(publisher);
			Article firstArticle = articles[0];
			string firstContent = Reader.Content(firstArticle.url);
			string text = Parser.Parse(firstContent, publisher);
			*/
			//Utility.CreateDatabase();
			//Utility.UpdateDatabase();
			var db = new SQLiteConnection(Utility.DATABASE_FILENAME);
			var firstArticle = db.Table<ArticleDB>().First();
			string[] words = Utility.Words(firstArticle.ID);
			string[] wordsToTakeIntoAccount = new string[Utility.NUMBER_OF_WORDS];
			Array.Copy(words, wordsToTakeIntoAccount, Utility.NUMBER_OF_WORDS);
			string firstClozeTest = DataProcessor.ClozeTest(firstArticle.Text, wordsToTakeIntoAccount);
			Console.WriteLine(firstClozeTest);
		}
	}
}