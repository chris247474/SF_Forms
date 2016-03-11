using System;

using Xamarin.Forms;
using System.Collections.Generic;
using System.Diagnostics;

namespace Secret_Files
{
	public class UserFeed : Feed
	{
		public UserFeed (List<PostItemStackLayout> PostsContent, string title):base(PostsContent, title)
		{
			MessagingCenter.Subscribe<UserFeed>(this, Values.POSTREQUEST, (args) =>{
				try{
					Debug.WriteLine("PostRequest Received");
					//FeedList.Add (args as PostItem);
					//refresh();
				}catch(Exception e){
					Debug.WriteLine ("Subscribe Error: "+e.Message);
				}
			});
		}
	}
}


