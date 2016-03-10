using System;

using Xamarin.Forms;
using System.Collections.Generic;

namespace Secret_Files
{
	public class Feed : ContentPage
	{
		protected List<PostItem> FeedList{ get; set;}
		public Feed (List<PostItem> FeedList, string title)
		{
			
			this.BackgroundColor = Color.White;
			this.Title = title;

			FeedList = new List<PostItem> {
				new PostItem ("postsample.png", "post 4", 
					"klah dfklash dfkladsh fklasdhfk lasdhfk ljashflka  hsdfklhasdfkl jhasdk lfha slkdfhaskldjfhalsasldk f haskldfh klasdfh  lkasjdfhlkasj fhlasjdfh lkasjdf")
				,new PostItem ("postsample.png", "post 4", 
					"klah dfklash dfkladsh fklasdhfk lasdhfk ljashflka  hsdfklhasdfkl jhasdk lfha slkdfhaskldjfhalsasldk f haskldfh klasdfh  lkasjdfhlkasj fhlasjdfh lkasjdf")
				,new PostItem ("postsample.png", "post 4", 
					"klah dfklash dfkladsh fklasdhfk lasdhfk ljashflka  hsdfklhasdfkl jhasdk lfha slkdfhaskldjfhalsasldk f haskldfh klasdfh  lkasjdfhlkasj fhlasjdfh lkasjdf")
				,new PostItem ("postsample.png", "post 4", 
					"klah dfklash dfkladsh fklasdhfk lasdhfk ljashflka  hsdfklhasdfkl jhasdk lfha slkdfhaskldjfhalsasldk f haskldfh klasdfh  lkasjdfhlkasj fhlasjdfh lkasjdf")
				,new PostItem ("postsample.png", "post 4", 
					"klah dfklash dfkladsh fklasdhfk lasdhfk ljashflka  hsdfklhasdfkl jhasdk lfha slkdfhaskldjfhalsasldk f haskldfh klasdfh  lkasjdfhlkasj fhlasjdfh lkasjdf")
				,new PostItem ("postsample.png", "post 4", 
					"klah dfklash dfkladsh fklasdhfk lasdhfk ljashflka  hsdfklhasdfkl jhasdk lfha slkdfhaskldjfhalsasldk f haskldfh klasdfh  lkasjdfhlkasj fhlasjdfh lkasjdf")
				,new PostItem ("postsample.png", "post 4", 
					"klah dfklash dfkladsh fklasdhfk lasdhfk ljashflka  hsdfklhasdfkl jhasdk lfha slkdfhaskldjfhalsasldk f haskldfh klasdfh  lkasjdfhlkasj fhlasjdfh lkasjdf")
				,new PostItem ("postsample.png", "post 4", 
					"klah dfklash dfkladsh fklasdhfk lasdhfk ljashflka  hsdfklhasdfkl jhasdk lfha slkdfhaskldjfhalsasldk f haskldfh klasdfh  lkasjdfhlkasj fhlasjdfh lkasjdf")
				,new PostItem ("postsample.png", "post 4", 
					"klah dfklash dfkladsh fklasdhfk lasdhfk ljashflka  hsdfklhasdfkl jhasdk lfha slkdfhaskldjfhalsasldk f haskldfh klasdfh  lkasjdfhlkasj fhlasjdfh lkasjdf")
				,new PostItem ("postsample.png", "post 4", 
					"klah dfklash dfkladsh fklasdhfk lasdhfk ljashflka  hsdfklhasdfkl jhasdk lfha slkdfhaskldjfhalsasldk f haskldfh klasdfh  lkasjdfhlkasj fhlasjdfh lkasjdf")
				,new PostItem ("postsample.png", "post 4", 
					"klah dfklash dfkladsh fklasdhfk lasdhfk ljashflka  hsdfklhasdfkl jhasdk lfha slkdfhaskldjfhalsasldk f haskldfh klasdfh  lkasjdfhlkasj fhlasjdfh lkasjdf")
				,new PostItem ("postsample.png", "post 4", 
					"klah dfklash dfkladsh fklasdhfk lasdhfk ljashflka  hsdfklhasdfkl jhasdk lfha slkdfhaskldjfhalsasldk f haskldfh klasdfh  lkasjdfhlkasj fhlasjdfh lkasjdf")
				,new PostItem ("postsample.png", "post 4", 
					"klah dfklash dfkladsh fklasdhfk lasdhfk ljashflka  hsdfklhasdfkl jhasdk lfha slkdfhaskldjfhalsasldk f haskldfh klasdfh  lkasjdfhlkasj fhlasjdfh lkasjdf")
				,new PostItem ("postsample.png", "post 4", 
					"klah dfklash dfkladsh fklasdhfk lasdhfk ljashflka  hsdfklhasdfkl jhasdk lfha slkdfhaskldjfhalsasldk f haskldfh klasdfh  lkasjdfhlkasj fhlasjdfh lkasjdf")
				,new PostItem ("postsample.png", "post 4", 
					"klah dfklash dfkladsh fklasdhfk lasdhfk ljashflka  hsdfklhasdfkl jhasdk lfha slkdfhaskldjfhalsasldk f haskldfh klasdfh  lkasjdfhlkasj fhlasjdfh lkasjdf")
				,new PostItem ("postsample.png", "post 4", 
					"klah dfklash dfkladsh fklasdhfk lasdhfk ljashflka  hsdfklhasdfkl jhasdk lfha slkdfhaskldjfhalsasldk f haskldfh klasdfh  lkasjdfhlkasj fhlasjdfh lkasjdf")

			};
			Util.SubscribeForPostRequests (FeedList);
			Content = Util.CreateScrollableFeedView (FeedList, "Search", title);
		}
		public void refresh (){
			this.Content = Util.CreateScrollableFeedView (FeedList, "Search", this.Title);
		}

	}
}


