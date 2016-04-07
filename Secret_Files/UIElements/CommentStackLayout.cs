using System;
using Xamarin.Forms;
using System.Diagnostics;

namespace Secret_Files
{
	public class CommentStackLayout:StackLayout
	{
		Image postImage;
		Label TextBody, PostTitle, React, Reply, Tag;
		StackLayout stack;
		TapGestureRecognizer ReactHandler, ReplyHandler;

		public string Text, ImageString, UserCommenter;

		public CommentStackLayout (PostItemStackLayout parentPost, PostItem post, string imgFile, string commentername, string bodytext, bool GrayedOut = false)
		{
			Text = bodytext;
			ImageString = imgFile;
			UserCommenter = commentername;

			this.BackgroundColor = Color.White;
			postImage = new Image{
				Source = imgFile,
				HorizontalOptions = LayoutOptions.Start,
				Aspect = Aspect.AspectFit,

			};
			PostTitle = new Label{
				Text = commentername,
				HorizontalOptions = LayoutOptions.Start,
				FontSize = (Device.GetNamedSize (NamedSize.Small, typeof(Label)) * 0.8)*0.8
			};
			TextBody = new Label{
				Text = commentername + "\t" + bodytext, 
				LineBreakMode = LineBreakMode.WordWrap,
				HorizontalTextAlignment = TextAlignment.Start,
				FontSize = (Device.GetNamedSize (NamedSize.Small, typeof(Label)) * 0.8)*0.9
			};
			React = new Label{ 
				FontSize = (Device.GetNamedSize (NamedSize.Small, typeof(Label)) * 0.8)*0.9,
				Text = "Like",
				TextColor = Color.Teal
			};
			Reply = new Label{ 
				FontSize = (Device.GetNamedSize (NamedSize.Small, typeof(Label)) * 0.8)*0.9,
				Text = "Reply",
				TextColor = Color.Teal
			};
			Tag = new Label {
				FontSize = (Device.GetNamedSize (NamedSize.Small, typeof(Label)) * 0.8)*0.9,
				Text = "@anonymousperson",
				TextColor = Color.Teal
			};

			ReactHandler = new TapGestureRecognizer ();
			ReactHandler.Tapped += (sender, e) => {
				Util.UpdatePostReactionCount (post);
				if(string.Equals (React.Text, Values.LIKE)){
					React.Text = Values.DISLIKE;
				}else{
					React.Text = Values.LIKE;
				}
			}; 
			var refertoself = this;
			ReplyHandler = new TapGestureRecognizer ();
			ReplyHandler.Tapped += (sender, e) => {
				Debug.WriteLine("Focusing for comment reply");
				parentPost.CommentEntry.Focus ();
				parentPost.CommentEntry.Text = "@"+commentername;
			};
			Reply.GestureRecognizers.Add (ReplyHandler);
			React.GestureRecognizers.Add (ReactHandler);

			stack = new StackLayout {
				Orientation = StackOrientation.Vertical,
				//Padding = new Thickness(30, 0, 20, 10),
				Children = {
					new StackLayout{
						Orientation = StackOrientation.Horizontal,
						Children = {postImage, 
							new StackLayout{
								Orientation = StackOrientation.Vertical,
								Children = {TextBody, new StackLayout{
										Orientation = StackOrientation.Horizontal,
										VerticalOptions = LayoutOptions.End,
										Children = {
											React, 
											Reply
										}
									}}
							}
						}
					}
				}
			};

			this.Children.Add (stack);

			if (GrayedOut) {
				this.Opacity = 0.3;//gray out while broadcast hasnt been received. solidify once received. add resend functions
			} else {
			}
		}
	}
}

