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
	public partial class CompleteArticles : ContentPage
	{
		public CompleteArticles (ArticleDB article)
		{
			InitializeComponent ();
			Content = new Label() { Text = article.Text };
		}
	}
}
