using System;

using Xamarin.Forms;
using System.Threading.Tasks;
using Secret_Files.Helpers;
using Acr.UserDialogs;
using System.Diagnostics;

namespace Secret_Files
{
	public class ProfilePicChooser : ContentPage
	{	
		GridWithSelectedImageString grid;
		public ProfilePicChooser ()
		{
			CreateView ();
		}
		void CreateView(){
			grid = new GridWithSelectedImageString ();
			UIBuilder.BuildProfilePicGrid (grid, new string[]{ "brownhairblackcollarmale.png" , "brownhaircaucasian.png", "orangeeyes.png", "stormtrooper.png"});
			this.BackgroundColor = Color.FromHex (Values.BACKGROUNDLIGHTSILVER);
			Content = new StackLayout { 
				BackgroundColor = Color.White,
				Padding = new Thickness(20),
				Children = {
					new Label{
						Text = "Choose an anonymous profile pic!",
						HorizontalTextAlignment = TextAlignment.Center,
						HorizontalOptions = LayoutOptions.Center,
						FontSize = Device.GetNamedSize (NamedSize.Large, typeof(Label))
					},
					new Label{
						Text = "To keep Secret Files and its community anonymous, we don't allow users to upload profile pictures. \nHave fun choosing a Secret Face!",
						HorizontalTextAlignment = TextAlignment.Center,
						HorizontalOptions = LayoutOptions.Center,
						FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label))
					},
					new ScrollView{Content = grid}
				}
			};
		}

	}
}


