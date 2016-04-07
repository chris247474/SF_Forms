using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Diagnostics;
using Refractored.XamForms.PullToRefresh;
using Secret_Files.Helpers;
using Acr.UserDialogs;
using System.Threading.Tasks;

namespace Secret_Files
{
	public static class Util
	{
		static SearchBar searchBar;
		static ScrollView ScrollFeed;


		public static string GetTrendingOrNewestIconPathname(){
			if (string.Equals (App.NewestOrTrending, Values.TRENDING)) {
				return "ic_whatshot_yellow_600_24dp.png";
			} else {
				return "ic_whatshot_white_24dp.png";
			}
		}
		public static async Task GetUserFeedback(){
			var result = await UserDialogs.Instance.PromptAsync(string.Format ("Hey {0}! How can we help you? :)", Settings.Username), "We look forward to hearing from you", "Send", "Cancel");
			if(result.Ok){
				//send to emails
			}
		}
		public static async Task ChangeProfilePic(Image img){
			try{
				string newprofilepic = GetImagePathFromImage(img);
				if(!string.IsNullOrWhiteSpace (newprofilepic)){
					Settings.ProfilePic = Util.FromPickerIconChooseProfileIcon (newprofilepic);
					Debug.WriteLine ("New profile pic is {0}", Settings.ProfilePic);
					UserDialogs.Instance.ShowSuccess ("Profile picture changed!", 2000);
				}else{
					UserDialogs.Instance.WarnToast ("Oops, something went wrong, pls try again!", null, 2000);
				}
			}catch(Exception e){
				Debug.WriteLine ("Problem changing profile pic: "+e.Message);
				UserDialogs.Instance.WarnToast ("Oops, something went wrong, pls try again!", null, 2000);
			}
		}
		public static List<GroupItem> AssignDefaultPicsToEmptyGroupPics(List<GroupItem> TempSearchItems){
			if (TempSearchItems == null) {
				return null;
			}
			var searchItemsArr = TempSearchItems.ToArray ();
			for (int c = 0; c < searchItemsArr.Length; c++) {
				if (string.IsNullOrWhiteSpace (searchItemsArr [c].groupImage)) {
					searchItemsArr[c].groupImage = Values.DEFAULTPIC;
				}
			}
			return TempSearchItems;
		}
		public static async Task ChangeGroupPic(GridWithSelectedImageString grid, Image img){
			try{
				string newprofilepic = GetImagePathFromImage(img);
				if(!string.IsNullOrWhiteSpace (newprofilepic)){
					//GroupItem.groupImage = Util.FromPickerIconChooseProfileIcon (newprofilepic);
					grid.StringImageSelected = newprofilepic;//Util.FromPickerIconChooseProfileIcon(newprofilepic);
					Debug.WriteLine ("New group pic is {0}", grid.StringImageSelected);
					UserDialogs.Instance.ShowSuccess ("Group picture changed!", 2000);
				}else{
					UserDialogs.Instance.WarnToast ("Oops, something went wrong, pls try again!", null, 2000);
				}
			}catch(Exception e){
				Debug.WriteLine ("Problem changing group pic: "+e.Message);
				UserDialogs.Instance.WarnToast ("Oops, something went wrong, pls try again!", null, 2000);
			}
		}
		public static string FromPickerIconChooseProfileIcon(string pickedicon){
			string[] s = pickedicon.Split ('.');
			Debug.WriteLine (s[0]+"icon.png");
			return s[0]+"icon.png";
		}
		public static string GetImagePathFromImage(Image objImage){
			if (objImage.Source is Xamarin.Forms.FileImageSource)
			{
				Xamarin.Forms.FileImageSource objFileImageSource = (Xamarin.Forms.FileImageSource)objImage.Source;
				return objFileImageSource.File;
			}
			return null;
		}
		public static async Task SaveNewUserName(){
			var result = await UserDialogs.Instance.PromptAsync("What's your preferred nickname?", "Change username", "OK", "Cancel");
			if(string.IsNullOrWhiteSpace(result.Text) || string.IsNullOrEmpty(result.Text)){
			}else {
				await ChangeUsername(result.Text);
			}
		}
		static async Task ChangeUsername(string newUsername){
			try{
				var user = await App.DataDB.GetUserByID (Settings.UserId);
				if(user != null){
					user.username = newUsername;
					await App.DataDB.SaveAccountTaskAsync (user);
					UserDialogs.Instance.ShowSuccess ("Nickname changed!", 2000);
					Settings.Username = newUsername;
				}else{
					UserDialogs.Instance.WarnToast ("I wasn't able to save your new username. Please try again in a bit");
				}
			}catch(Exception){
				UserDialogs.Instance.WarnToast ("I wasn't able to save your new username. Please try again in a bit");
			}
		}
		public static void UpdatePostReactionCount(PostItem post){
			Debug.WriteLine ("Adding to reactionCount");
			post.reactionCount++;
			App.DataDB.SavePostItemsTaskAsync (post);
		}
		public static string GenerateRandomUsername()
		{
			string rv = "";

			char[] lowers = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'j', 'k', 'm', 'n', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
			char[] uppers = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
			char[] numbers = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

			int l = lowers.Length;
			int u = uppers.Length;
			int n = numbers.Length;

			Random random = new Random();

			rv += lowers[random.Next(0, l)].ToString();
			rv += lowers[random.Next(0, l)].ToString();
			rv += lowers[random.Next(0, l)].ToString();

			rv += uppers[random.Next(0, u)].ToString();
			rv += uppers[random.Next(0, u)].ToString();
			rv += uppers[random.Next(0, u)].ToString();

			rv += numbers[random.Next(0, n)].ToString();
			rv += numbers[random.Next(0, n)].ToString();
			rv += numbers[random.Next(0, n)].ToString();

			return rv;
		}
		public static void NewGroupNotif(CreateNewGroupPage page){
			MessagingCenter.Send(page, Values.REFRESH);
		}
		public static void LoggedInNotif(App page){
			MessagingCenter.Send(page, Values.REFRESH);
		}
		public static string BuildShoutText(string shoutScopeName){
			return Values.SHOUTSTRING + shoutScopeName.ToLower () + Values.WONTKNOW;
		}

		public static List<PostItemStackLayout> LoadFeedDataIntoFeedList(List<PostItem> PostData, List<CommentItem> CommentData, string groupid){//load comments into feed here
			PostItemStackLayout postTemp = null;
			CommentItem[] CommentDataArray = null;

			if (CommentData != null && CommentData.Count > 0) {//check if this group has any comments in the posts
				CommentDataArray = CommentData.ToArray ();
			} else {
				CommentDataArray = new CommentItem[]{ };
			}

			try{
				var PostDataArray = PostData.ToArray (); 

				List<PostItemStackLayout> FeedList = new List<PostItemStackLayout> ();

				if(PostData == null){
					Debug.WriteLine ("LoadFeedDataIntoFeedList error: Azure data passed to parameter is null");
				}else{
					for(int c = 0;c < PostDataArray.Length;c++){
						postTemp = new PostItemStackLayout(PostDataArray[c]);
						//add comments from server to postTemp
						if(CommentDataArray.Length > 0){//if groupfeed has any comments in the posts
							for(int ctr = 0;ctr < CommentDataArray.Length;ctr++){//add all the comments in their respective posts
								if(string.Equals (CommentDataArray[ctr].PostID, PostDataArray[c].ID)){
									postTemp.stack.Children.Add (new CommentStackLayout(postTemp, PostDataArray[c], CommentDataArray[ctr].UserImage, 
										CommentDataArray[ctr].UserCommentName, CommentDataArray[ctr].CommentText));//build commentstacklayouts and add to UI under respective poststacklayouts
								}
							}
						}
						FeedList.Add (postTemp);
					}
					return FeedList;
				}
			}catch(Exception e){
				Debug.WriteLine ("User has no posts in this page: "+e.Message);
				//UserDialogs.Instance.WarnToast ("Your data connection is a bit slow right now");
			}

			return null;
		}
		public static BoxView CreateSeparator(Color borderColor, double opacity){
			return new BoxView () { Color = Color.Gray, HeightRequest = 1, Opacity = opacity  };
		}
		public static SearchBar CreateSearchBar(GroupsPage page){
			searchBar = new SearchBar {
				Placeholder = "Search here"
			};
			searchBar.TextChanged += (sender, e) => {
				page.SearchGroups ();
			};
			return searchBar;
		}
		public static SearchBar CreateSearchBar(){
			searchBar = new SearchBar {
				Placeholder = "Search"
			};
			searchBar.TextChanged += (sender, e) => {
				
			};
			return searchBar;
		}
		public static StackLayout CreateScrollableFeedView(List<PostItemStackLayout> PostsContent, string placeholder, string scopeName, string groupid = null){
			if (PostsContent != null && PostsContent.Count > 0) {
				Debug.WriteLine ("Posts found");
				ScrollFeed = new ScrollView {
					Content = CreateFeed (PostsContent)
				};

				var refreshView = new PullToRefreshLayout { 
					VerticalOptions = LayoutOptions.FillAndExpand, 
					HorizontalOptions = LayoutOptions.FillAndExpand,  
					Content = ScrollFeed, 
					RefreshColor = Color.FromHex ("#3498db")
				};  
				//Set pull-to-refresh Bindings 
				//refreshView.SetBinding<GroupFeed> (PullToRefreshLayout.IsRefreshingProperty, vm => vm.IsBusy, BindingMode.OneWay); 
				//refreshView.SetBinding<GroupFeed> (PullToRefreshLayout.IsRefreshingProperty, new Xamarin.Forms.Binding(){Path="IsBusy", Mode = BindingMode.OneWay}); 
				//refreshView.SetBinding<GroupFeed>(PullToRefreshLayout.RefreshCommandProperty, vm => vm.RefreshCommand);

				return new StackLayout { 
					Orientation = StackOrientation.Vertical, 
					Children = { new ShoutBar ("postsample.png", scopeName, groupid),
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
		public static StackLayout CreateFeed(List<PostItemStackLayout> list){
			var stack = new StackLayout {
				Orientation = StackOrientation.Vertical,
				Children = {
					new StackLayout{
						BackgroundColor = Color.White,
						Children = {CreateSearchBar()}
					}
				}
			};

			if (list == null) {
				//UserDialogs.Instance.InfoToast ("No posts here yet", null, 2000);//change to text background?
			} else {
				var listArr = list.ToArray (); 

				for(int c = 0;c < listArr.Length;c++){
					stack.Children.Add (listArr[c]);
				}
			}

			return stack;
		}

		public static StackLayout CreateFeedLayout(List<PostItemStackLayout> list){
			StackLayout stack = new StackLayout{ 
				Orientation = StackOrientation.Vertical
			};
			if (list == null) {
				//UserDialogs.Instance.InfoToast ("No posts here yet", null, 2000);//change to text background?
			} else {
				var listArr = list.ToArray (); 

				for(int c = 0;c < listArr.Length;c++){
					stack.Children.Add (listArr[c]);
				}
			}
			return stack;
		}
		public static void Search(){}
	}
}

