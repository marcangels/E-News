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
		public ResultsView (ScoreDB score)
		{
			InitializeComponent ();
			Label lab = new Label()
			{
				Text = $"You have a score of {score.score}/10."
			};
			StackLayout mainStack = new StackLayout()
			{
				Children = { lab }
			};
			Content = mainStack;
		}
	}
}
