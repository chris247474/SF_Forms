using System;
using Xamarin.Forms;
using XLabs.Forms.Controls;


namespace Secret_Files
{
	public class NewGroupBar:StackLayout
	{
		public Image img;
		public String GroupName{
			get{
				return groupNameEntry.Text;
			}
		}
		TapGestureRecognizer tapHandler;
		Entry groupNameEntry;

		public NewGroupBar ()
		{
			

			this.BackgroundColor = Color.White;
			tapHandler = new TapGestureRecognizer{
				NumberOfTapsRequired = 1
			};
			tapHandler.Tapped += (sender, e) => {
				//choose image
			};

			img = new Image{
				Source = "cameraplaceholder28x28.jpg",//default empty pic
				HorizontalOptions = LayoutOptions.Start
			};img.GestureRecognizers.Add (tapHandler);

			groupNameEntry = new Entry {
				Text = "What do you want to call this group?",
				TextColor = Color.Silver,
				//FontSize = Font.SystemFontOfSize (NamedSize.Medium).FontSize
			};
			groupNameEntry.Focused += (sender, e) => {
				groupNameEntry.Text = "";
			};

			//Padding = new Thickness (30, 20, 20, 15);

			this.Orientation = StackOrientation.Horizontal;
			this.Children.Add (img);
			this.Children.Add (groupNameEntry);

			return;
		}
	}
}

