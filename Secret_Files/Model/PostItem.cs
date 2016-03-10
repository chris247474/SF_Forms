using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace Secret_Files
{
	public class PostItem:StackLayout
	{
		Image postImage;
		Label DatePosted;

		Label TextBody, PostTitle;

		public PostItem (string imgFile, string posttitle, string bodytext)
		{
			this.BackgroundColor = Color.White;
			postImage = new Image{
				Source = imgFile,
				HorizontalOptions = LayoutOptions.Start
			};
			PostTitle = new Label{
				Text = posttitle,
				HorizontalOptions = LayoutOptions.StartAndExpand
			};
			TextBody = new Label{
				Text = bodytext,
				LineBreakMode = LineBreakMode.WordWrap,
				XAlign = TextAlignment.Start
			};

			var stack = new StackLayout {
				Orientation = StackOrientation.Vertical,
				Padding = new Thickness(30, 20, 20, 30),
				Children = {new StackLayout{
						Orientation = StackOrientation.Horizontal,
						Children = {postImage, PostTitle}
					}, TextBody}
			};

			this.Children.Add (stack);
		}
	}
}

