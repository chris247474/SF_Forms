using System;

using Xamarin.Forms;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Refractored.XamForms.PullToRefresh;
using System.Windows.Input;
using System.ComponentModel;

namespace Secret_Files
{
	public class Feed : ContentPage, INotifyPropertyChanged
	{
		/*public string groupid;
		public PullToRefreshViewModel refreshHandler;
		protected PullToRefreshLayout refreshView;

		public Feed (string title, string groupid = null)
		{
			refreshHandler = new PullToRefreshViewModel (this);
			BindingContext = refreshHandler;
			this.groupid = groupid;
			this.BackgroundColor = Color.FromHex (Values.BACKGROUNDLIGHTSILVER);
			this.Title = title;
			PopulateContent ();
		}
		public void PopulateContent(){
			var preloadStack = new StackLayout { 
				Orientation = StackOrientation.Vertical,
				Children = {
					new ShoutBar ("postsample.png", this.Title, groupid)
				}
			};
			Content = preloadStack;
			refreshHandler.ExecuteRefreshCommand();
		}

		ScrollView ScrollFeed;
		public StackLayout CreateScrollableFeedView(List<PostItemStackLayout> PostsContent, string placeholder, string scopeName, string groupid = null){
			if (PostsContent != null && PostsContent.Count > 0) {
				Debug.WriteLine ("Posts found");
				ScrollFeed = new ScrollView {
					Content = Util.CreateFeed (PostsContent)
				};

				refreshView = new PullToRefreshLayout { 
					VerticalOptions = LayoutOptions.FillAndExpand, 
					HorizontalOptions = LayoutOptions.FillAndExpand,  
					Content = ScrollFeed, 
					RefreshColor = Color.FromHex ("#3498db"),
					IsPullToRefreshEnabled = true
				};  
				Debug.WriteLine ("Posts feed pull to refresh enabled: "+ refreshView.IsPullToRefreshEnabled.ToString ());
				refreshView.SetBinding (PullToRefreshLayout.IsRefreshingProperty, new Xamarin.Forms.Binding(){Path="IsBusy", Mode = BindingMode.OneWay});
				refreshView.SetBinding(PullToRefreshLayout.RefreshCommandProperty, new Xamarin.Forms.Binding(){Path="RefreshCommand"});

				return new StackLayout { 
					Orientation = StackOrientation.Vertical, 
					Children = { 
						new ShoutBar ("postsample.png", scopeName, groupid),

						refreshView
					}
				};
			} else {
				Label NoPostsLabel = new Label{
					Text = "Be the first to start this thread!",
					TextColor = Color.Silver,
					FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label)),
					VerticalOptions = LayoutOptions.Center,
					HorizontalOptions = LayoutOptions.Center
				};
				return new StackLayout { 
					Orientation = StackOrientation.Vertical, 
					Children = { 
						new ShoutBar ("postsample.png", scopeName, groupid),
						NoPostsLabel
					}
				};
			}
		}
		public void SearchGroups()
		{
		}*/
	}
}


