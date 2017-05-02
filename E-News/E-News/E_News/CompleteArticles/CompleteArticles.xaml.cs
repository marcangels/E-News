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
		public List<PickerViewModel> LstPickerViewModel;
		private ArticleDB article;
		private string[] wordsToTakeIntoAccount;

		public CompleteArticles (ArticleDB pArticle)
		{
			InitializeComponent ();
			article = pArticle;
			GenerateViewAsync();
			LstPickerViewModel = new List<PickerViewModel>();
		}

		private StackLayout GenerateView()
		{
			try
			{
				StackLayout mainStack = new StackLayout();
				var margin = new Thickness(5, 5, 5, 20);
				Label title = new Label()
				{
					Text = article.Title,
					FontAttributes = FontAttributes.Bold,
					FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
					VerticalTextAlignment = TextAlignment.Center,
					HorizontalTextAlignment = TextAlignment.Center,
					Margin = new Thickness(5, 5, 5, 20)
			}; 
				mainStack.Children.Add(title);
				Label currentLabel = new Label() { FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label)), Margin = new Thickness(5,5,5,5) };
				var tuple = GetExerciceText();
				string[] Words = tuple.Item1.Split(' ');
				List<string> CorrectWords = tuple.Item2;
				mainStack.Children.Add(currentLabel);
				string word;
				int j = 0;
				for (int i = 0; i < Words.Length; i++)
				{
					word = Words[i];
					if (Regex.IsMatch(word, "[,;:.!?]*_[,;:.!?]*"))
					{
						if (word.Length > 1)
						{
							int index = word.IndexOf('_');
							string start = word.Substring(0, index);
							string end = word.Substring(index + 1);

						}
						PickerViewModel pickerVM = new PickerViewModel()
						{
							lstWords = CorrectWords,
							CorrectWord = CorrectWords[j]
						};
						LstPickerViewModel.Add(pickerVM);
						j++;
						Picker picker = new Picker()
						{
							BindingContext = pickerVM,
							ItemsSource = wordsToTakeIntoAccount
						};
						picker.SetBinding(Picker.SelectedItemProperty, "SelectedWord");

						mainStack.Children.Add(picker);
						currentLabel = new Label() { FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label)), Margin = new Thickness(5, 5, 5, 5) };
						mainStack.Children.Add(currentLabel);
					}
					else
					{
						currentLabel.Text += word + " ";
					}

				}
				Button ValidateButton = new Button()
				{
					Text = "Check your choices"
				};
				ValidateButton.Clicked += ValidateHandler;
				mainStack.Children.Add(ValidateButton);
				return mainStack;
			} catch(Exception e)
			{
				Debug.WriteLine(e);
				return null;
			}
		}

		public async void ValidateHandler(object sender, EventArgs e)
		{
			int score = 0;
			LstPickerViewModel.ForEach(ele => {
				Debug.WriteLine($"{ele.SelectedWord}:{ele.CorrectWord} => {ele.IsCorrect}");
				if (ele.IsCorrect) score++;
			});
			await Navigation.PushAsync(new ResultsView(new ScoreDB() { score = score, timestamp = DateTime.Now.Ticks }));
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
			wordsToTakeIntoAccount = new string[Utility.NUMBER_OF_WORDS];
			Array.Copy(words, wordsToTakeIntoAccount, Utility.NUMBER_OF_WORDS);
			var firstClozeTest = DataProcessor.ClozeTest(article.Text, wordsToTakeIntoAccount);
			return firstClozeTest;
		}
		


		public class PickerViewModel : INotifyPropertyChanged
		{
			public List<string> lstWords;
			private string selectedWord;
			public string SelectedWord
			{
				get { return selectedWord; }
				set
				{
					selectedWord = value;
					RaisePropertyChanged("SelectedWord");
				}
			}
			public int Position { get; set; }
			public string CorrectWord { get; set; } = "";
			public bool IsCorrect => SelectedWord == CorrectWord;

			public event PropertyChangedEventHandler PropertyChanged;

			private void RaisePropertyChanged(String property)
			{
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
			}
		}
	}
}
