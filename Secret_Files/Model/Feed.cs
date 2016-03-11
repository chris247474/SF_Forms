using System;

using Xamarin.Forms;
using System.Collections.Generic;
using System.Diagnostics;

namespace Secret_Files
{
	public class Feed : ContentPage
	{
		protected List<PostItemStackLayout> FeedList{ get; set;}
		public Feed (List<PostItemStackLayout> FeedList, string title)
		{
			
			this.BackgroundColor = Color.White;
			this.Title = title;


			/*FeedList = new List<PostItemStackLayout> {
				new PostItemStackLayout ("postsample.png", "post 4", 
					"klah dfklash dfkladsh fklasdhfk lasdhfk ljashflka  hsdfklhasdfkl jhasdk lfha slkdfhaskldjfhalsasldk f haskldfh klasdfh  lkasjdfhlkasj fhlasjdfh lkasjdf")
				,new PostItemStackLayout ("postsample.png", "post 4", 
					"klah dfklash dfkladsh fklasdhfk lasdhfk ljashflka  hsdfklhasdfkl jhasdk lfha slkdfhaskldjfhalsasldk f haskldfh klasdfh  lkasjdfhlkasj fhlasjdfh lkasjdf")
				,new PostItemStackLayout ("postsample.png", "post 4", 
					"klah dfklash dfkladsh fklasdhfk lasdhfk ljashflka  hsdfklhasdfkl jhasdk lfha slkdfhaskldjfhalsasldk f haskldfh klasdfh  lkasjdfhlkasj fhlasjdfh lkasjdf")
				,new PostItemStackLayout ("postsample.png", "post 4", 
					"klah dfklash dfkladsh fklasdhfk lasdhfk ljashflka  hsdfklhasdfkl jhasdk lfha slkdfhaskldjfhalsasldk f haskldfh klasdfh  lkasjdfhlkasj fhlasjdfh lkasjdf")
				,new PostItemStackLayout ("postsample.png", "post 4", 
					"klah dfklash dfkladsh fklasdhfk lasdhfk ljashflka  hsdfklhasdfkl jhasdk lfha slkdfhaskldjfhalsasldk f haskldfh klasdfh  lkasjdfhlkasj fhlasjdfh lkasjdf")
				,new PostItemStackLayout ("postsample.png", "post 4", 
					"klah dfklash dfkladsh fklasdhfk lasdhfk ljashflka  hsdfklhasdfkl jhasdk lfha slkdfhaskldjfhalsasldk f haskldfh klasdfh  lkasjdfhlkasj fhlasjdfh lkasjdf")
				,new PostItemStackLayout ("postsample.png", "post 4", 
					"klah dfklash dfkladsh fklasdhfk lasdhfk ljashflka  hsdfklhasdfkl jhasdk lfha slkdfhaskldjfhalsasldk f haskldfh klasdfh  lkasjdfhlkasj fhlasjdfh lkasjdf")
				,new PostItemStackLayout ("postsample.png", "post 4", 
					"klah dfklash dfkladsh fklasdhfk lasdhfk ljashflka  hsdfklhasdfkl jhasdk lfha slkdfhaskldjfhalsasldk f haskldfh klasdfh  lkasjdfhlkasj fhlasjdfh lkasjdf")
				,new PostItemStackLayout ("postsample.png", "post 4", 
					"klah dfklash dfkladsh fklasdhfk lasdhfk ljashflka  hsdfklhasdfkl jhasdk lfha slkdfhaskldjfhalsasldk f haskldfh klasdfh  lkasjdfhlkasj fhlasjdfh lkasjdf")
				,new PostItemStackLayout ("postsample.png", "post 4", 
					"klah dfklash dfkladsh fklasdhfk lasdhfk ljashflka  hsdfklhasdfkl jhasdk lfha slkdfhaskldjfhalsasldk f haskldfh klasdfh  lkasjdfhlkasj fhlasjdfh lkasjdf")
				,new PostItemStackLayout ("postsample.png", "post 4", 
					"klah dfklash dfkladsh fklasdhfk lasdhfk ljashflka  hsdfklhasdfkl jhasdk lfha slkdfhaskldjfhalsasldk f haskldfh klasdfh  lkasjdfhlkasj fhlasjdfh lkasjdf")
				,new PostItemStackLayout ("postsample.png", "post 4", 
					"klah dfklash dfkladsh fklasdhfk lasdhfk ljashflka  hsdfklhasdfkl jhasdk lfha slkdfhaskldjfhalsasldk f haskldfh klasdfh  lkasjdfhlkasj fhlasjdfh lkasjdf")
				,new PostItemStackLayout ("postsample.png", "post 4", 
					"klah dfklash dfkladsh fklasdhfk lasdhfk ljashflka  hsdfklhasdfkl jhasdk lfha slkdfhaskldjfhalsasldk f haskldfh klasdfh  lkasjdfhlkasj fhlasjdfh lkasjdf")
				,new PostItemStackLayout ("postsample.png", "post 4", 
					"klah dfklash dfkladsh fklasdhfk lasdhfk ljashflka  hsdfklhasdfkl jhasdk lfha slkdfhaskldjfhalsasldk f haskldfh klasdfh  lkasjdfhlkasj fhlasjdfh lkasjdf")
				,new PostItemStackLayout ("postsample.png", "post 4", 
					"klah dfklash dfkladsh fklasdhfk lasdhfk ljashflka  hsdfklhasdfkl jhasdk lfha slkdfhaskldjfhalsasldk f haskldfh klasdfh  lkasjdfhlkasj fhlasjdfh lkasjdf")
				,new PostItemStackLayout ("postsample.png", "post 4", 
					"klah dfklash dfkladsh fklasdhfk lasdhfk ljashflka  hsdfklhasdfkl jhasdk lfha slkdfhaskldjfhalsasldk f haskldfh klasdfh  lkasjdfhlkasj fhlasjdfh lkasjdf")

			};*/
			PopulateContent ();
		}
		public async void PopulateContent(){
			Content = Util.CreateScrollableFeedView (Util.LoadFeedDataIntoFeedList(await App.DataDB.GetPostItemsAsync ()), "Search", this.Title);
		}
		public void refresh (){
			this.Content = Util.CreateScrollableFeedView (Util.LoadFeedDataIntoFeedList(App.DataDB.GetPostItemsAsync ().Result), "Search", this.Title);
		}

	}
}


