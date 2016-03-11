using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Diagnostics;

namespace Secret_Files
{
	public static class Util
	{
		static SearchBar searchBar;
		static ScrollView ScrollFeed;

		public static string BuildShoutText(string shoutScopeName){
			return Values.SHOUTSTRING + shoutScopeName.ToLower () + Values.WONTKNOW;
		}
		public static List<PostItemStackLayout> LoadFeedDataIntoFeedList(List<PostItem> PostData){
			var PostDataArray = PostData.ToArray (); 
			List<PostItemStackLayout> FeedList = new List<PostItemStackLayout> ();

			if(PostData == null){
				Debug.WriteLine ("LoadFeedDataIntoFeedList error: Azure data passed to parameter is null");
			}else{
				for(int c = 0;c < PostDataArray.Length;c++){
					FeedList.Add (new PostItemStackLayout("postsample.png", PostDataArray[c].Title, PostDataArray[c].Body));
				}
				return FeedList;
			}
			return null;
		}
		public static StackLayout CreateScrollableFeedView(List<PostItemStackLayout> PostsContent, string placeholder, string scopeName){
			searchBar = new SearchBar {
				Placeholder = placeholder
			};
			searchBar.TextChanged += (sender, e) => {
				Util.Search ();
			};

			ScrollFeed = new ScrollView {
				Content = CreateFeed(PostsContent)
			};

			return new StackLayout { 
				Orientation = StackOrientation.Vertical, 
				Children = { new ShoutBar ("postsample.png", scopeName),
					ScrollFeed
				}
			};
		}
		public static StackLayout CreateFeed(List<PostItemStackLayout> list){
			var listArr = list.ToArray (); 
			var stack = new StackLayout {
				Orientation = StackOrientation.Vertical,
				BackgroundColor = Color.Silver,
				Padding = new Thickness (0, 8, 0, 15),
				Children = {
					new StackLayout{
						BackgroundColor = Color.White,
						Children = {searchBar}
					}
				}
			};
			for(int c = 0;c < listArr.Length;c++){
				stack.Children.Add (listArr[c]);
			}
			return stack;
		}
		public static void Search(){}
	}
}

