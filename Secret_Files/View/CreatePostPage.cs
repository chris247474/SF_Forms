using System;

using Xamarin.Forms;
using System.Diagnostics;
using Secret_Files.Helpers;

namespace Secret_Files
{
	
	public class CreatePostPage : ContentPage
	{
		ScrollView scrollView;
		ToolbarItem nvm, shout;
		Editor textArea;
		string groupid, GroupName;
		PullToRefreshViewModel refresher;

		public CreatePostPage (PullToRefreshViewModel refresher, string title, string groupname, string groupid = null)
		{
			this.refresher = refresher;
			this.GroupName = groupname;
			this.groupid = groupid;
			this.Title = title;
			this.BackgroundColor = Color.White;

			Content = CreatePostLayout ();
		}

		public StackLayout CreatePostLayout(){

			nvm = new ToolbarItem{
				Text = "Cancel",
				Icon = "ic_clear_white_24dp.png"
			};
			nvm.Clicked += (object sender, EventArgs e) => {
				Navigation.PopAsync ();
			};
			shout = new ToolbarItem{
				Text = "Shout",
				Icon = "ic_send_white_24dp.png"
			};
			shout.Clicked += async (object sender, EventArgs e) => {
				//post to feed
				if(!string.IsNullOrEmpty (groupid)){
					try{
						Debug.WriteLine ("user {0} saved post to group: {1}", Settings.UserId, groupid);
						PostItem post = new PostItem{Body = textArea.Text, Title = Settings.Username, GroupID = this.groupid, UserId = Settings.UserId, PostImage = Settings.ProfilePic};
						await App.DataDB.SavePostItemsTaskAsync (post);
						PostItem postwID = await App.DataDB.GetSinglePostByPostTextTitleUserIDGroupID(post.Body, post.Title, post.UserId, post.GroupID);
						Debug.WriteLine ("Generated post ID is {0}", postwID.ID);
						App.ChatClient.Send (postwID);
					}catch(Exception){
						Debug.WriteLine ("post error");
						this.refresher.ExecuteRefreshCommand ();
					}

				}else{
					Debug.WriteLine ("user {0} saved personal post", Settings.UserId);
					await App.DataDB.SavePostItemsTaskAsync (new PostItem{Body = textArea.Text, Title = Settings.Username, UserId = Settings.UserId});
				}

				//send message to refresh feed
				//Debug.WriteLine ("SENDING REFRESH MESSAGE");
				//MessagingCenter.Send(this, Values.REFRESH);

				Navigation.PopAsync ();
			};
			this.ToolbarItems.Add (shout);
			this.ToolbarItems.Add (nvm);

			textArea = new Editor {
				Text = this.Title+ ". No one will know",
				Keyboard = Keyboard.Create(KeyboardFlags.All),
				//Keyboard = Keyboard.Chat,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				//HeightRequest = scrollView.Height - 10
			};
			textArea.Focused += (sender, e) => {
				if(string.Equals(textArea.Text, this.Title+ ". No one will know")){
					textArea.Text = "";
				}
			};
			textArea.TextChanged += (sender, e) => {
				
			};

			scrollView = new ScrollView{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Content = textArea
			};

			return new StackLayout { 
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Children = {
					scrollView
				}
			};
		}
		
	}
}


