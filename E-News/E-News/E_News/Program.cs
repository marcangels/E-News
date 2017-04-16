using System;
using System.Collections.Generic;
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
			Utility.DeleteDatabase();
			Console.WriteLine("Database deleted.");
			Utility.CreateDatabase();
			Console.WriteLine("Database created.");
			Utility.UpdateDatabase();
			Console.WriteLine("Database updated.");
			var db = new SQLiteConnection(Utility.DATABASE_FILENAME);
			var firstArticle = db.Table<ArticleDB>().ElementAt(1);
			Dictionary<string, double> wordsAndScores = Utility.Words(firstArticle.ID);
			string[] words = new string[wordsAndScores.Count];
			wordsAndScores.Keys.CopyTo(words, 0);
			string[] wordsToTakeIntoAccount = new string[Utility.NUMBER_OF_WORDS];
			Array.Copy(words, wordsToTakeIntoAccount, Utility.NUMBER_OF_WORDS);
			string firstClozeTest = DataProcessor.ClozeTest(firstArticle.Text, wordsToTakeIntoAccount);
			Console.WriteLine(firstClozeTest);
			Console.WriteLine("Words: ");
			for (int i = 0; i < wordsToTakeIntoAccount.Length; i++)
			{
				string word = wordsToTakeIntoAccount[i];
				Console.WriteLine("TF*IDF({0}) = {1}", word, wordsAndScores[word]);
			}
		}
	}
}