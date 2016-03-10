using System;

using Xamarin.Forms;

namespace Secret_Files
{
	
	public class CreatePostPage : ContentPage
	{
		ScrollView scrollView;
		ToolbarItem nvm, shout;
		Editor textArea;

		public CreatePostPage (string title)
		{
			this.Title = title;
			this.BackgroundColor = Color.White;

			Content = CreatePostLayout ();
		}

		public StackLayout CreatePostLayout(){

			nvm = new ToolbarItem{
				Text = "Cancel"
			};
			nvm.Clicked += (object sender, EventArgs e) => {
				Navigation.PopAsync ();
			};
			shout = new ToolbarItem{
				Text = "Shout"
			};
			shout.Clicked += (object sender, EventArgs e) => {
				//post to users page
				Util.RequestPostToFeed(new PostItem("postsample.png", "user post test", textArea.Text));
				Navigation.PopAsync ();
			};
			this.ToolbarItems.Add (shout);
			this.ToolbarItems.Add (nvm);

			textArea = new Editor {
				Text = Values.SHOUTSTRING 
			};
			textArea.Focused += (sender, e) => {
				textArea.Text = "";
			};

			scrollView = new ScrollView{
				Content = textArea
			};

			return new StackLayout { 
				Children = {
					scrollView
				}
			};
		}
		
	}
}


