using System;
using Xamarin.Forms;
using System.Collections.Generic;
//using Acr.UserDialogs;

namespace Secret_Files
{
	public static class Util
	{
		static SearchBar searchBar;
		static ScrollView ScrollFeed;

		public static void RequestPostToFeed(PostItem post){
			MessagingCenter.Send(typeof(CreatePostPage), Values.POSTREQUEST, post);
		}
		public static void SubscribeForPostRequests(List<PostItem> FeedList){
			MessagingCenter.Subscribe<PostItem>(typeof(Feed), Values.POSTREQUEST, (args) =>{
				try{
					FeedList.Add (args as PostItem);
				}catch(Exception e){
					//UserDialogs.Instance.InfoToast ("Couldn't add post to feed", "Feed.SubscribeForPostRequests() :"+e.Message, 4000);
				}
			});
		}
		public static string BuildShoutText(string shoutScopeName){
			return Values.SHOUTSTRING + shoutScopeName.ToLower () + Values.WONTKNOW;
		}
		public static StackLayout CreateScrollableFeedView(List<PostItem> PostsContent, string placeholder, string scopeName){
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
		public static StackLayout CreateFeed(List<PostItem> list){
			var listArr = list.ToArray (); 
			var stack = new StackLayout {
				Orientation = StackOrientation.Vertical,
				BackgroundColor = Color.Silver,
				Padding = new Thickness (0, 8, 0, 30),
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

