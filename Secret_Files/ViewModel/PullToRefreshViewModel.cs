using System;
using Xamarin.Forms;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Input;
using System.Threading.Tasks;
using Acr.UserDialogs;

namespace Secret_Files
{
	public class PullToRefreshViewModel:INotifyPropertyChanged
	{
		GroupFeed page;
		public PullToRefreshViewModel (GroupFeed page)
		{
			this.page = page;
		}

		bool isBusy;

		public bool IsBusy
		{
			get { return isBusy; }
			set
			{
				Debug.WriteLine ("IsBusy");
				if (isBusy == value)
					return;

				isBusy = value;
				OnPropertyChanged("IsBusy");
			}
		}
		private ICommand refreshCommand;
		public ICommand RefreshCommand{
			get { 
				Debug.WriteLine ("RefreshCommand");
				return refreshCommand ?? (refreshCommand = new Command(async () => await ExecuteRefreshCommand())); 
			}
		}

		public async Task ExecuteRefreshCommand(GroupFeed feed = null){
			Debug.WriteLine ("Started ExecuteRefreshCommand");
			if (IsBusy) return; 

			IsBusy = true;

			Debug.WriteLine ("Rebuilding UI post list");
			if (string.Equals (App.NewestOrTrending, Values.TRENDING)) {
				//then sort posts by trending
				page.Content = page.CreateScrollableFeedView (Util.LoadFeedDataIntoFeedList (await App.DataDB.GetPostItemsByGroupOrderByPopularAsync (page.groupid), 
					await App.DataDB.GetCommentsByGroupIDAsync (page.groupid), page.groupid), "Search", page.Title, page.groupid);
			} else if(string.Equals (App.NewestOrTrending, Values.NEWEST)){
				//then order by most recently posted
				page.Content = page.CreateScrollableFeedView (Util.LoadFeedDataIntoFeedList (await App.DataDB.GetPostItemsByGroupOrderByRecentAsync (page.groupid), 
					await App.DataDB.GetCommentsByGroupIDAsync (page.groupid), page.groupid), "Search", page.Title, page.groupid);
			}

			IsBusy = false; 
			Debug.WriteLine ("ExecuteRefreshCommand done");

			DisplayToUserTrendingOrNewest ();

			//start refresh interval for posts
			App.ChatClient.ListenForNewPosts (this, page.feedPosts, page.groupid);
		}
		void DisplayToUserTrendingOrNewest(){
			if (string.Equals (App.NewestOrTrending, Values.TRENDING)) {
				UserDialogs.Instance.InfoToast ("Trending Secrets", null, 1000);
			} else {
				UserDialogs.Instance.InfoToast ("Newest Secrets", null, 1000);
			}
		}

		#region INotifyPropertyChanged implementation

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		public void OnPropertyChanged(string propertyName)
		{
			Debug.WriteLine ("Property changed");
			if (PropertyChanged == null)
				return;

			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}

