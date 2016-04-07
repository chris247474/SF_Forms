using System;

using Xamarin.Forms;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Secret_Files
{
	public class UserFeed : Feed
	{
		public UserFeed (List<PostItem> PostsContent, string title)//:base(title)
		{
			MessagingCenter.Subscribe<CreatePostPage> (this, Values.REFRESH, async (args) => { 
				Debug.WriteLine ("REFRESH MESSAGE RECEIVED");
				await this.refresh (); 
			});
		}
		public async Task refresh (){
			//this.Content = Util.CreateScrollableFeedView (Util.LoadFeedDataIntoFeedList (await App.DataDB.GetPostItemsAsync ()), "Search", this.Title);
		}
	}
}


