using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace E_News
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CompleteArticles : ContentPage
	{
		public List<EntryViewModel> LstEntryViewModel;
		private ArticleDB article;

		public CompleteArticles (ArticleDB pArticle)
		{
			InitializeComponent ();
			article = pArticle;
			GenerateViewAsync();
			LstEntryViewModel = new List<EntryViewModel>();
		}

		private StackLayout GenerateView()
		{
			StackLayout mainStack = new StackLayout();
			Label currentLabel = new Label();
			var tuple = GetExerciceText();
			string[] Words = tuple.Item1.Split(' ');
			List<string> CorrectWords = tuple.Item2;
			mainStack.Children.Add(currentLabel);
			string word;
			int j = 0;
			for (int i = 0; i < Words.Length; i++)
			{
				word = Words[i]; // TODO il y a des cas où on a "_-to-_"
				if (Regex.IsMatch(word, "[,;:.!?]*_[,;:.!?]*"))
				{
					if (word.Length > 1)
					{
						int index = word.IndexOf('_');
						string start = word.Substring(0, index);
						string end = word.Substring(index + 1);
						
					}

					EntryViewModel entryVM = new EntryViewModel() {
						Position = i,
						WordOrig = CorrectWords[j]
					};
					j++;
					LstEntryViewModel.Add(entryVM);
					Entry entry = new Entry()
					{
						BindingContext = entryVM
					};
					entry.SetBinding(Entry.TextProperty, "EntryText");
					mainStack.Children.Add(entry);
					currentLabel = new Label();
					//while (j < tmp.Length)
					//{
					//	currentLabel.Text += tmp[j];
					//	j++;
					//}
					mainStack.Children.Add(currentLabel);
				}
				else
				{
					currentLabel.Text += word + " ";
				}
			}
			Button ValidateButton = new Button()
			{
				Text = "Validate"
			};
			ValidateButton.Clicked += (s, e) =>
				LstEntryViewModel.ForEach(ele => Debug.WriteLine($"{ele.EntryText}:{ele.WordOrig} => {ele.IsCorrect}"));
			mainStack.Children.Add(ValidateButton);
			return mainStack;
		}

		private void GetCorrectWords()
		{

		}

		private async void GenerateViewAsync()
		{
			StackLayout stack = await Task<StackLayout>.Factory.StartNew(() => GenerateView());
			Content = new ScrollView()
			{
				Content = GenerateView()
			};
		}

		private Tuple<string, List<string>> GetExerciceText()
		{
			Dictionary<string, double> wordsAndScores = Utility.Words(article.ID);
			string[] words = new string[wordsAndScores.Count];
			wordsAndScores.Keys.CopyTo(words, 0);
			string[] wordsToTakeIntoAccount = new string[Utility.NUMBER_OF_WORDS];
			Array.Copy(words, wordsToTakeIntoAccount, Utility.NUMBER_OF_WORDS);
			var firstClozeTest = DataProcessor.ClozeTest(article.Text, wordsToTakeIntoAccount);
			return firstClozeTest;
		}

		private int GetStartPonctuation(string str)
		{
			return 0;
		}

		public class EntryViewModel : INotifyPropertyChanged
		{
			private string entryText;
			public string EntryText
			{
				get { return entryText; }
				set
				{
					entryText = value;
					RaisePropertyChanged("EntryText");
				}
			}
			public int Position { get; set; }
			public string WordOrig { get; set; } = "";
			public bool IsCorrect => EntryText == WordOrig;

			public event PropertyChangedEventHandler PropertyChanged;

			private void RaisePropertyChanged(String property)
			{
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
			}
		}
	}
}
