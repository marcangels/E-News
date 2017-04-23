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
		public MainPage()
		{
			InitializeComponent();

			Title = "E-News";

			Button button = new Button() { Text = "Complete articles" };
			button.Clicked += async (s,e) => await Navigation.PushAsync(new ListArticles());
			Content = new StackLayout
			{
				Children = { button }
			};
		}
	}
}
