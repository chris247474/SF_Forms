using System;
using Xamarin.Forms;

namespace Secret_Files
{
	public class ShoutBar:StackLayout
	{
		Image img;
		Label ShoutLabel;
		TapGestureRecognizer tapHandler;

		public ShoutBar (string imgName, string shoutScopeName)
		{
			this.BackgroundColor = Color.White;
			tapHandler = new TapGestureRecognizer{
				NumberOfTapsRequired = 1
			};
			tapHandler.Tapped += (sender, e) => {
				Navigation.PushAsync (new CreatePostPage("Confess/Shout/Whatever to "+shoutScopeName.ToLower ()));
			};

			img = new Image{
				Source = imgName,
				HorizontalOptions = LayoutOptions.Start
			};
			ShoutLabel = new Label{ 
				Text = Util.BuildShoutText(shoutScopeName),
				TextColor = Color.Silver,
				XAlign = TextAlignment.Start,
				YAlign = TextAlignment.Center
			};ShoutLabel.GestureRecognizers.Add (tapHandler);

			Padding = new Thickness (30, 20, 20, 15);

			this.Orientation = StackOrientation.Horizontal;
			this.Children.Add (img);
			this.Children.Add (ShoutLabel);

			return;
		}
	}
}

