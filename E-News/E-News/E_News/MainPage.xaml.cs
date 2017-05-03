using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace E_News
{
	public partial class MainPage : ContentPage
	{
		StackLayout mainStack;
		Label labProgress;

		public MainPage()
		{
			InitializeComponent();
			//ManageDB();
			Title = "E-News";
			Appearing += AppearingHandler;

			Button button = new Button() { Text = "Let's go!" };
			button.Clicked += async (s,e) => await Navigation.PushAsync(new ListArticles());
			labProgress = new Label()
			{
				Text = "",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				FontAttributes = FontAttributes.Bold
			};
			mainStack = new StackLayout
			{
				Children = { button, labProgress }
			};
			Content = mainStack;
			//ShowProgression();
		}

		private async void ShowProgression()
		{
			await Task.Factory.StartNew(() => ManageDB());
			labProgress.Text = Utility.Progression();
		}

		private void AppearingHandler(object sender, EventArgs e)
		{
			ShowProgression();
		}

		private void ManageDB()
		{
			if (!File.Exists(Utility.DATABASE_FILENAME))
			{
				Utility.CreateDatabase();
				Debug.WriteLine("Database created.");
			}
		}
	}
}
