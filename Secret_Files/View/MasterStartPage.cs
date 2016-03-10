using System;

using Xamarin.Forms;
using System.Collections.Generic;

namespace Secret_Files
{
	public class MasterStartPage : MasterDetailPage
	{
		ListView listView;

		public MasterStartPage ()
		{
			this.Detail = new GlobalFeed(null, "Worldwide Secret Feed");
			this.Master = new ContentPage { 
				Title = "Who's Secrets do you wanna read?",
				Content = createView ()
			};
		}
		StackLayout createView(){
			List<string> list = new List<string> ();
			list.Add ("Profile");
			list.Add ("Test Group");
			list.Add ("Confession Booth");

			listView = new ListView ()
			{
				ItemsSource = list
			};
			listView.ItemSelected += (sender, e) => {
				string selected = (string)e.SelectedItem;

				switch(selected){
				case "Test Group":
					Navigation.PushAsync (new GroupFeed(null, "Test Group"));
					break;
				case "Confession Booth":
					Navigation.PushAsync (new UserFeed(null, "Your Confession Booth (under review)"));
					break;
				}

				//deselect
				((ListView)sender).SelectedItem = null; 
			};

			return new StackLayout{
				Orientation = StackOrientation.Vertical,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.StartAndExpand,
				Children = { 
					listView
				}
			};
		}
	}
}


