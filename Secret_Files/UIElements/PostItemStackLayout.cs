using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Diagnostics;
using Secret_Files.Helpers;
using System.Threading.Tasks;
using Acr.UserDialogs;

namespace Secret_Files
{
	public class PostItemStackLayout:StackLayout
	{
		Image postImage;
		CommentItem comment;
		int stackCtr = 0;
		Label TextBody, PostTitle, CommentLabel, LikeLabel;
		public Entry CommentEntry;
		Entry commentEntry;
		int postedCount;
		string commentTextToSave;
		PostItem post;

		public StackLayout stack;
		TapGestureRecognizer CommentTapHandler, MoreReactionsGesture, LikeGesture;

		public PostItemStackLayout (PostItem post)
		{
			this.BackgroundColor = Color.White;
			postImage = new Image{
				Source = post.PostImage,
				HorizontalOptions = LayoutOptions.Start
			};
			PostTitle = new Label{
				Text = post.Title,
				HorizontalOptions = LayoutOptions.StartAndExpand
			};
			TextBody = new Label{
				Text = post.Body,
				LineBreakMode = LineBreakMode.WordWrap,
				HorizontalTextAlignment = TextAlignment.Start
			};

			CommentLabel = new Label{
				Text = "Comment",
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.Center,
				FontSize = (Device.GetNamedSize (NamedSize.Small, typeof(Label)) * 0.8)
			};
			LikeLabel = new Label{
				Text = "Like",
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.Center,
				FontSize = (Device.GetNamedSize (NamedSize.Small, typeof(Label)) * 0.8)
			};
			MoreReactionsGesture = new TapGestureRecognizer {
				NumberOfTapsRequired = 3,
				//show all possible reactions to post
			};
			LikeGesture = new TapGestureRecognizer {
				NumberOfTapsRequired = 1,
			};
			LikeGesture.Tapped += (sender, e) => {
				Util.UpdatePostReactionCount (post);
				if(string.Equals (LikeLabel.Text, Values.LIKE)){
					LikeLabel.Text = Values.DISLIKE;
				}else{
					LikeLabel.Text = Values.LIKE;
				}
			};
			LikeLabel.GestureRecognizers.Add (LikeGesture);

			var CommentLayout = new StackLayout{
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.Center,
				Padding = new Thickness(7.5, 0, 0, 7.5),
				Children = { 
					LikeLabel, CommentLabel
				}
			};

			stack = new StackLayout {
				Orientation = StackOrientation.Vertical,
				Padding = new Thickness(30, 20, 20, 20),
				Children = {
					new StackLayout{
						Orientation = StackOrientation.Horizontal,
						Children = {postImage, PostTitle}
					}, TextBody, 
					// draws a separator line and space of 5 above and below the separator    
					new BoxView() { Color = Color.White, HeightRequest = 5  },
					new BoxView() { Color = Color.Gray, HeightRequest = 1, Opacity = 0.5},
					CommentLayout, 
					new BoxView() { Color = Color.White, HeightRequest = 5  },
				}
			};
			CommentEntry = CommentCreatorLayout (post, stack, Settings.ProfilePic, post.Title, post.Body, post.GroupID, post.ID);
			this.Children.Add (stack);
			this.Children.Add (CommentEntry);

			CommentTapHandler = new TapGestureRecognizer();
			CommentTapHandler.Tapped += (sender, e) => {
				CommentEntry.Focus ();
			};
			CommentLabel.GestureRecognizers.Add (CommentTapHandler);

			App.ChatClient.ListenForNewComments (this, stack, post);
		}
		public Entry CommentCreatorLayout(PostItem post, StackLayout parent, string imgFile, string posttitle, string bodytext, string groupid, string postid){
			MessagingCenter.Subscribe<CommentStackLayout, string> (this, Values.FOCUSONCOMMENTENTRY, (sender, arg) => {
				Debug.WriteLine ("Replying to comment");
				CommentEntry.Focus ();
				CommentEntry.Text = "@"+" "+arg;
			});

			commentEntry = new Entry{
				Placeholder = Values.COMMENTPLACEHOLDER,
				PlaceholderColor = Color.Gray,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				FontSize = (Device.GetNamedSize (NamedSize.Small, typeof(Label)) * 0.8),
				Keyboard = Keyboard.Create(KeyboardFlags.All),
				TextColor = Color.Gray
			};

			commentEntry.Focused += (sender, e) => {
				//MessagingCenter.Send (this, Values.Busy);
				if(string.Equals (commentEntry.Text, commentEntry.Placeholder)){
					commentEntry.Text = string.Empty;
				}
			};

			commentEntry.Unfocused += (sender, e) => {
				//fire completed?
			};
			commentEntry.TextChanged += async (sender, e) => {
				this.commentTextToSave = commentEntry.Text;
			};
			commentEntry.Completed += async (object sender, EventArgs e) => {
				//save
				Debug.WriteLine ("Finished commenting");

				parent.Children.Add (new CommentStackLayout (this, post, Settings.ProfilePic, Settings.Username, commentEntry.Text, true));

				//save to db
				comment = new CommentItem{CommentText = this.commentTextToSave, GroupID = groupid, PostID = postid, 
					UserCommentID = Settings.UserId, reactionCount = 0, UserCommentName = Settings.Username, UserImage = Settings.ProfilePic};
				await App.DataDB.SaveCommentItemsTaskAsync (comment);

				//Debug.WriteLine ("broadcasting comment");
				//App.ChatClient.SendComment (comment);
				SendOrRetrySendingComment(comment);

				Util.UpdatePostReactionCount(post);
				commentEntry.Text = Values.COMMENTPLACEHOLDER;
				//MessagingCenter.Send (this, Values.NotBusy);
			};



			return commentEntry;
		}
		async void SendOrRetrySendingComment(CommentItem comment){
			Debug.WriteLine ("broadcasting comment");
			try{
				await App.ChatClient.SendComment (comment);
			}catch(Exception e){
				Debug.WriteLine ("problem sending comment, retrying: {0}", e.Message);
				await App.ChatClient.SendComment (comment);
				try{
					await App.ChatClient.SendComment (comment);
				}catch(Exception ex){
					Debug.WriteLine ("problem sending comment, retrying: {0}", ex.Message);
					UserDialogs.Instance.ShowError ("Couldn't connect to Secret Files. Your connection isn't so great now");
				}
			}
		}
	}
}

