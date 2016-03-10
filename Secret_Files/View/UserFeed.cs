using System;

using Xamarin.Forms;
using System.Collections.Generic;

namespace Secret_Files
{
	public class UserFeed : Feed
	{
		public UserFeed (List<PostItem> PostsContent, string title):base(PostsContent, title)
		{
		}
	}
}


