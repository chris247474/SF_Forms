using System;

using Xamarin.Forms;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Refractored.XamForms.PullToRefresh;
using System.ComponentModel;
using System.Threading;

namespace Secret_Files
{
	public class GroupFeed : ContentPage
	{
		public string groupid, groupname;
		public PullToRefreshViewModel refreshHandler;
		protected PullToRefreshLayout refreshView;
		ToolbarItem TrendingTBI, NewestTBI, ShoutTBI;
		public ToolbarItem Post;
		public StackLayout feedPosts;
		ScrollView ScrollFeed; 

		public GroupFeed (string groupname, string groupid = null){
			MessagingCenter.Subscribe<PostItemStackLayout> (this, Values.NotBusy, (args) => { 
				Debug.WriteLine ("setting isbusy false");
				IsBusy = false;
			});
			MessagingCenter.Subscribe<PostItemStackLayout> (this, Values.Busy, (args) => { 
				Debug.WriteLine ("setting isbusy true");
				IsBusy = true;
			});

			ListenForRefresh ();

			refreshHandler = new PullToRefreshViewModel (this);
			BindingContext = refreshHandler;
			this.groupid = groupid;
			this.groupname = groupname;
			this.BackgroundColor = Color.FromHex (Values.BACKGROUNDLIGHTSILVER);
			this.Title = groupname;
			PopulateContent ();
		}
		public async void PopulateContent(){
			InitTBI ();
			var preloadStack = new StackLayout { 
				Orientation = StackOrientation.Vertical,
				Children = {
					//Util.CreateSearchBar()
				}
			};
			Content = preloadStack;

			refreshHandler.ExecuteRefreshCommand(this);
		}
		void StartIntervalRefresh(){
			Device.StartTimer(new TimeSpan(0,0,30), () => {
				if(!((BindingContext as PullToRefreshViewModel).IsBusy) && !IsBusy){
					refreshHandler.ExecuteRefreshCommand ();
				}else{
					Debug.WriteLine ("refresher is busy, dont refresh");
				}
				return true;
			});
		}

		public StackLayout CreateScrollableFeedView(List<PostItemStackLayout> PostsContent, string placeholder, string scopeName, string groupid = null){
			StackLayout stack = new StackLayout ();

			if (PostsContent != null && PostsContent.Count > 0) {
				Debug.WriteLine ("Posts found");
				feedPosts = Util.CreateFeed (PostsContent);
				ScrollFeed = new ScrollView {
					Content = feedPosts
				};

				refreshView = new PullToRefreshLayout {  
					VerticalOptions = LayoutOptions.FillAndExpand, 
					HorizontalOptions = LayoutOptions.FillAndExpand,  
					Content = ScrollFeed, 
					RefreshColor = Color.FromHex ("#3498db"),
					IsPullToRefreshEnabled = true
				};  
				refreshView.SetBinding (PullToRefreshLayout.IsRefreshingProperty, new Xamarin.Forms.Binding(){Path="IsBusy", Mode = BindingMode.OneWay});
				refreshView.SetBinding(PullToRefreshLayout.RefreshCommandProperty, new Xamarin.Forms.Binding(){Path="RefreshCommand"});

				stack = new StackLayout { 
					Orientation = StackOrientation.Vertical, 
					Children = { refreshView }
				};


			} else {
				Label NoPostsLabel = new Label{
					Text = "Be the first to start this secret file!",
					TextColor = Color.Silver,
					FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label)),
					VerticalOptions = LayoutOptions.Fill,
					HorizontalOptions = LayoutOptions.Fill
				};
				refreshView = new PullToRefreshLayout {  
					VerticalOptions = LayoutOptions.Center, 
					HorizontalOptions = LayoutOptions.Center,  
					Content = NoPostsLabel, 
					RefreshColor = Color.FromHex ("#3498db"),
					IsPullToRefreshEnabled = true
				};  
				stack = new StackLayout{ 
					Orientation = StackOrientation.Vertical, 
					VerticalOptions = LayoutOptions.FillAndExpand, 
					HorizontalOptions = LayoutOptions.FillAndExpand,  
					Children = {
						refreshView
					}
				};
			}

			return UIBuilder.AddFloatingActionButtonToStackLayout (stack, "ic_add_white_24dp.png", new Command(async () => {
				Navigation.PushAsync (new CreatePostPage(refreshHandler, "Add a Secret to "+this.Title, groupname, groupid));
			}), Color.FromHex (Values.PURPLE), Color.FromHex (Values.GOOGLEBLUE));
		}

		void InitTBI(){
			TrendingTBI = new ToolbarItem ("Trending", Util.GetTrendingOrNewestIconPathname (), () => {
				if(string.Equals (App.NewestOrTrending, Values.TRENDING)){
					App.NewestOrTrending = Values.NEWEST;
					TrendingTBI.Icon = Values.NEWESTICON;
				}else if(string.Equals (App.NewestOrTrending, Values.NEWEST)){
					App.NewestOrTrending = Values.TRENDING;
					TrendingTBI.Icon = Values.TRENDINGICON;
				}
				refreshHandler.ExecuteRefreshCommand();
				Util.DisplayToUserTrendingOrNewest ();
			});
			NewestTBI = new ToolbarItem ("Newest", "", () => {
				App.NewestOrTrending = Values.NEWEST;
				refreshHandler.ExecuteRefreshCommand();
			});

			ShoutTBI = new ToolbarItem ("Shout", "ic_create_white_24dp.png", () => {
				Navigation.PushAsync (new CreatePostPage(refreshHandler, "Add a Secret to "+this.Title, groupname, groupid));
			});
			//this.ToolbarItems.Add (ShoutTBI);
			this.ToolbarItems.Add (TrendingTBI);
		}
		public void SearchGroups()
		{
		}
		void ListenForRefresh(){
			MessagingCenter.Subscribe<CreatePostPage> (this, Values.REFRESH, async (args) => { 
				Debug.WriteLine ("REFRESH MESSAGE RECEIVED");
				await this.refreshHandler.ExecuteRefreshCommand ();
			});
		}

		protected override void OnAppearing(){
			Debug.WriteLine ("Appearing");
			App.NavPage.BarBackgroundColor = Color.FromHex (Values.GOOGLEBLUE);
		}
		protected override void OnDisappearing(){
			Debug.WriteLine ("OnDisappearing");
			App.NavPage.BarBackgroundColor = Color.FromHex (Values.PURPLE);
		}
	}
}

