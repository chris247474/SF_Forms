using System;
using Xamarin.Forms;

namespace Secret_Files
{
	public class ShoutBar:StackLayout
	{
		Image img;
		Label ShoutLabel;
		TapGestureRecognizer tapHandler;

		public ShoutBar (string imgName, string shoutScopeName, string groupid = null)
		{
			this.BackgroundColor = Color.White;
			tapHandler = new TapGestureRecognizer{
				NumberOfTapsRequired = 1
			};
			tapHandler.Tapped += (sender, e) => {
				//Navigation.PushAsync (new CreatePostPage("Add a Secret to "+shoutScopeName, groupid));
			};

			img = new Image{
				Source = imgName,
				HorizontalOptions = LayoutOptions.Start
			};
			ShoutLabel = new Label{ 
				Text = Util.BuildShoutText(shoutScopeName),
				TextColor = Color.Silver,
				HorizontalTextAlignment = TextAlignment.Start,
				VerticalTextAlignment = TextAlignment.Center
			};ShoutLabel.GestureRecognizers.Add (tapHandler);

			Padding = new Thickness (30, 10, 20, 15);

			this.Orientation = StackOrientation.Vertical;
			this.Children.Add (new StackLayout{
				Orientation = StackOrientation.Horizontal,
				Children = {img, ShoutLabel}
			});
			//this.Children.Add (Util.CreateSeparator (Color.Gray, 1));

			return;
		}
	}
}

