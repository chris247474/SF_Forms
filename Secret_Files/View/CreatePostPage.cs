using System;

using Xamarin.Forms;

namespace Secret_Files
{
	
	public class CreatePostPage : ContentPage
	{
		ScrollView scrollView;
		ToolbarItem nvm, shout;
		Editor textArea;
		string postTitle;

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
			shout.Clicked += async (object sender, EventArgs e) => {
				//post to users page
				await App.DataDB.SaveTaskAsync (new PostItem{Body = textArea.Text, Title = "test title"});
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


