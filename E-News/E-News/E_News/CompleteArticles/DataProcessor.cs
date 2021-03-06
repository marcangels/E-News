﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace E_News
{
	public static class DataProcessor
	{
		public static Dictionary<string,double> TfIdf(List<string> texts)
		{
			string originalTargetText = string.Copy(texts[0]);
			for (var i = 0; i < texts.Count; i++)
			{
				texts[i] = texts[i].ToLower();
			}
			string targetText = string.Copy(texts[0]);

			var assembly = typeof(DataProcessor).GetTypeInfo().Assembly;
			Stream stream = assembly.GetManifestResourceStream(Utility.STOP_WORDS_FILENAME);
			string text = "";
			using (var reader = new System.IO.StreamReader(stream))
			{
				text = reader.ReadToEnd();
			}

			var stopWords = text.Split(new[] { ", " }, StringSplitOptions.None);
			Dictionary<string, double> result = new Dictionary<string, double>();
			var words = Regex
				.Matches(targetText, @"\b[\w']*\b")
				.Cast<Match>()
				.Select(m => m.Value)
				.ToArray();
			Dictionary<string, int> wordCounts = new Dictionary<string, int>();
			foreach(string word in words)
			{
				var cleanedWord = word.ToLower().Trim();
				if(wordCounts.ContainsKey(word))
				{
					wordCounts[cleanedWord] = wordCounts[cleanedWord] + 1;
				}
				else
				{
					wordCounts[cleanedWord] = 1;
				}
			}
			foreach (string word in wordCounts.Keys)
			{
				var tf = (double)wordCounts[word] / (double)words.Length;
				var idf = Math.Log((double)texts.Count / (double)Utility.CountAppearance(texts, word));
				result[word] = tf * idf;
			}
			Regex wordRegex = new Regex(@"^[a-zA-Z]+$");
			result = result
				.OrderByDescending(pair => pair.Value)
				.Where(pair => wordRegex.IsMatch(pair.Key))
				.Where(pair => !stopWords.Contains(pair.Key))
                .Where(pair => !Regex.IsMatch(originalTargetText, string.Concat(pair.Key.Substring(0, 1).ToUpper(), pair.Key.Substring(1))))
				.Distinct()
				.Take(Utility.NUMBER_OF_WORDS)
				.ToDictionary(pair => pair.Key, pair => pair.Value);
			return result;
		}

		public static Tuple<string, List<string>> ClozeTest(string text, string[] words)
		{
			string tmp = text.ToLower();
			Dictionary<string, int> dicWords = new Dictionary<string, int>();
			foreach(string word in words)
			{
                var index = tmp.IndexOf(word, StringComparison.Ordinal);
				dicWords[word] = index;
				Debug.WriteLine($"test: {index}, {word}");
				tmp = tmp.Remove(index, word.Length);
				tmp = tmp.Insert(index, "_");
				text = text.Remove(index, word.Length);
				text = text.Insert(index, "_");
			}
			var tmpDicWords = dicWords.OrderBy(pair => pair.Value);
			List<string> lstWords = new List<string>();
			foreach (var item in tmpDicWords)
			{
				lstWords.Add(item.Key);
			}
			return Tuple.Create(text, lstWords);
		}
	}
}