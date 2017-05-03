using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace E_News
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ResultsView : ContentPage
	{
		ScoreDB Score;

		public ResultsView (ScoreDB score)
		{
			Score = score;
			var db = new SQLiteConnection(Utility.DATABASE_FILENAME);
			db.Insert(Score);
			InitializeComponent ();
			GenerateView();
		}

		private async void GenerateView()
		{
			Thickness margin = new Thickness(5, 5, 5, 5);
			Label lab = new Label()
			{
				Text = $"You have a score of {Score.Score}/10.",
				Margin = margin
			};
			ProgressBar progressBar = new ProgressBar()
			{
				Progress = 0,
				Margin = margin
			};
			//progressBar.PropertyChanged += (s, e) =>
			//{
			//	ProgressBar prog = s as ProgressBar;
			//	if (e.PropertyName == "Progress")
			//	{
			//		if (prog.Progress < 0.25)
			//			 = Color.Red;
			//		else if (prog.Progress < 0.5)
			//			prog.BackgroundColor = Color.Orange;
			//		else if (prog.Progress < 0.75)
			//			prog.BackgroundColor = Color.Yellow;
			//		else
			//			prog.BackgroundColor = Color.Green;
			//	}
			//};
			StackLayout mainStack = new StackLayout()
			{
				Children = { progressBar }
			};
			Content = mainStack;
			await progressBar.ProgressTo(((double)Score.Score) / 10, 1500, Easing.CubicIn);
			mainStack.Children.Add(lab);
		}
	}
}
