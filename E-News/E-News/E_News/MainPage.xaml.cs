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

            StackLayout mainStack = new StackLayout();
            Utility.DeleteDatabase();
            Console.WriteLine("Database deleted.");
            Utility.CreateDatabase();
            Console.WriteLine("Database created.");
            Utility.UpdateDatabase();
            Console.WriteLine("Database updated.");
            var db = new SQLiteConnection(Utility.DATABASE_FILENAME);
            var articles = db.Table<ArticleDB>();
			foreach (var item in articles)
			{
				mainStack.Children.Add(new Label() { Text = item.Title });
			}
            
            Content = mainStack;
        }
	}
}
