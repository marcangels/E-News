﻿using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Net;
using System.IO;
using SQLite;
using System.Diagnostics;

namespace E_News
{
	public static class Utility
	{
#if __IOS__
		public const string STOP_WORDS_FILENAME = @"E_News.iOS.StopWords.csv";
#endif
#if __ANDROID__
		public static string STOP_WORDS_FILENAME = "E_News.Droid.StopWords.csv";
#endif
#if WINDOWS_PHONE
		public const string STOP_WORDS_FILENAME = @"E_News.WinPhone.StopWords.csv";
#endif

		public static string DATABASE_FILENAME = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "db");
		public static string IMAGE_DIRECTORY = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Images");
		public const int NUMBER_OF_WORDS = 10;
		public static string[] PUBLISHERS = { "techcrunch", "ars-technica" };

		public static int IndexOfNth(string target, string toSearch, int n)
		{
			var result = -1;
			for (int i = 0; i < n; i++)
			{
				result = target.IndexOf(toSearch, result + 1, System.StringComparison.Ordinal);
				if (result == -1)
				{
					break;
				}
			}
			return result;
		}

		public static int Count(string target, string toSearch)
		{
			var result = 0;
			var index = target.IndexOf(toSearch, System.StringComparison.Ordinal);
			while (index != -1)
			{
				result++;
				index = target.IndexOf(toSearch, index + 1, System.StringComparison.Ordinal);
			}
			return result;
		}

		public static int CountAppearance(List<string> texts, string word)
		{
			var result = 0;
			foreach (string text in texts)
			{
				if (text.Contains(word))
				{
					result++;
				}
			}
			return result;
		}

		public static long DateToTicks(string date)
		{
			var dateSplit = date.Split('-');
			int year = Int32.Parse(dateSplit[0]);
			int month = Int32.Parse(dateSplit[1]);
			int day = Int32.Parse(dateSplit[2].Substring(0, 2));
			var convertedDate = new DateTime(year, month, day);
			return convertedDate.Ticks;
		}

		public static void DownloadImage(string url, int id)
		{
			WebClient webClient = new WebClient();
			webClient.DownloadFile(url, IMAGE_DIRECTORY + id);
		}

		public static void DeleteDatabase()
		{
			File.Delete(Utility.DATABASE_FILENAME);
		}

		public static void CreateDatabase()
		{
			var db = new SQLiteConnection(Utility.DATABASE_FILENAME);
			db.CreateTable<ArticleDB>();
			db.CreateTable<WordDB>();
		}

		public static void UpdateDatabase()
		{
			var db = new SQLiteConnection(Utility.DATABASE_FILENAME);
			List<string> presentArticlesTitles = new List<string>();
			List<string> presentArticlesTexts = new List<string>();
			var presentArticles = db.Table<ArticleDB>();
			var presentArticlesEnumerator = presentArticles.GetEnumerator();
			while (presentArticlesEnumerator.MoveNext())
			{
				presentArticlesTitles.Add(presentArticlesEnumerator.Current.Title);
				presentArticlesTexts.Add(presentArticlesEnumerator.Current.Text);
			}
			int lastId = presentArticlesTitles.Count;
			lastId++;
			var articleDB = new ArticleDB();
            List<GuardianArticle> articles = Reader.GuardianArticles();
            foreach (GuardianArticle article in articles)
            {
                if (!presentArticlesTitles.Contains(article.webTitle) && article.type.Equals("article"))
                {
                    articleDB.Title = article.webTitle;
                    articleDB.Ticks = Utility.DateToTicks(article.webPublicationDate);
                    articleDB.Text = Parser.ParseGuardian(article.fields.body);
                    string lowerCaseText = articleDB.Text.ToLower();
                    List<string> tfIdfList = new List<string>() { articleDB.Text };
                    foreach (string presentText in presentArticlesTexts)
                    {
                        tfIdfList.Add(presentText);
                    }
                    Dictionary<string, double> tfIdfs = DataProcessor.TfIdf(tfIdfList);
                    foreach (string word in tfIdfs.Keys)
                    {
                        WordDB wordDB = new WordDB()
                        {
                            ArticleID = lastId,
                            Word = word,
                            TfIdfScore = tfIdfs[word],
                            StringIndex = lowerCaseText.IndexOf(word, StringComparison.Ordinal)
                        };
                        db.Insert(wordDB);
                    }
                    db.Insert(articleDB);
                    presentArticlesTexts.Add(articleDB.Text);
                    lastId++;
                }
            }
			// Premier article, tf*idfs = 0
			db.Query<ArticleDB>("DELETE FROM WordDB WHERE ArticleId = 1");
			string firstLowerCaseText = presentArticlesTexts[0].ToLower();
			Dictionary<string, double> tfIdfsFirstArticle = DataProcessor.TfIdf(presentArticlesTexts);
			foreach (string word in tfIdfsFirstArticle.Keys)
			{
				WordDB wordDB = new WordDB() { ArticleID = 1, Word = word, TfIdfScore = tfIdfsFirstArticle[word], StringIndex = firstLowerCaseText.IndexOf(word, StringComparison.Ordinal) };
				db.Insert(wordDB);
			}
		}

		public static Dictionary<string, double> Words(int articleId)
		{
			var db = new SQLiteConnection(Utility.DATABASE_FILENAME);
			var data = db.Query<WordDB>("SELECT Word, TfIdfScore FROM WordDB WHERE ArticleId = ? ORDER BY TfIdfScore DESC", articleId);
			var enumerator = data.GetEnumerator();
			var result = new Dictionary<string, double>();
			while (enumerator.MoveNext())
			{
				result[enumerator.Current.Word] = enumerator.Current.TfIdfScore;
			}
			return result;

		}
	}
}