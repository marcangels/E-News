using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace E_News
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ListArticles : ContentPage
	{
		public ListArticles()
		{
			InitializeComponent ();
			BindingContext = new ListArticlesViewModel();
		}

		void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
			=> ((ListView)sender).SelectedItem = null;

		async void Handle_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem == null)
				return;

			await Navigation.PushAsync(new CompleteArticles((ArticleDB)e.SelectedItem));

			//Deselect Item
			((ListView)sender).SelectedItem = null;
		}
	}



	class ListArticlesViewModel : INotifyPropertyChanged
	{
		public ObservableCollection<ArticleDB> Articles { get; }

		public ListArticlesViewModel()
		{
			Articles = new ObservableCollection<ArticleDB>();
			RefreshDataCommand = new Command(
				async () => await RefreshData()
			);
			RefreshDataCommand.Execute(null);
		}

		public ICommand RefreshDataCommand { get; }

		async Task RefreshData()
		{
			IsBusy = true;
			//Load Data Here
			await Task.Run(() => {
				Articles.Clear();
				Utility.DeleteDatabase();
				Debug.WriteLine("Database deleted.");
				Utility.CreateDatabase();
				Debug.WriteLine("Database created.");
				Utility.UpdateDatabase();
				Debug.WriteLine("Database updated.");
				var db = new SQLiteConnection(Utility.DATABASE_FILENAME);
				var tmp = db.Table<ArticleDB>();
				foreach (var item in tmp)
				{
					Articles.Add(item);
				};
			});

			IsBusy = false;
		}

		bool busy;
		public bool IsBusy
		{
			get { return busy; }
			set
			{
				busy = value;
				OnPropertyChanged();
				((Command)RefreshDataCommand).ChangeCanExecute();
			}
		}


		public event PropertyChangedEventHandler PropertyChanged;
		void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
