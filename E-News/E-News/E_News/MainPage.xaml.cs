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

		public MainPage()
		{
			InitializeComponent();
			//ManageDB();
			Title = "E-News";

			Button button = new Button() { Text = "Fill in the gaps" };
			button.Clicked += async (s,e) => await Navigation.PushAsync(new ListArticles());
			mainStack = new StackLayout
			{
				Children = { button }
			};
			Content = mainStack;
			GenerateView();
		}

		private async void GenerateView()
		{
			await Task.Factory.StartNew(() => ManageDB());
			string progress = Utility.Progression();
			Label lab = new Label()
			{
				Text = progress,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				FontAttributes = FontAttributes.Bold
			};
			mainStack.Children.Add(lab);
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
