using System;

using Xamarin.Forms;
using System.Collections.Generic;
using Secret_Files.Helpers;
using Acr.UserDialogs;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Secret_Files
{
	public class SettingsPage : ContentPage
	{
		ListView listView;
		Page page;
		TapGestureRecognizer changePic, changeName;

		public SettingsPage (Page page)
		{Debug.WriteLine ("in constructor");
			this.page = page;
			CreateView ();
		}
		void CreateView(){
			Debug.WriteLine ("in CreateView");
			this.BackgroundColor = Color.FromHex (Values.BACKGROUNDLIGHTSILVER);
			this.Title = "Your Settings";

			changePic = new TapGestureRecognizer ();
			changePic.Tapped += (sender, e) => {
				page.Navigation.PushAsync (new ProfilePicChooser());
			};
			changeName = new TapGestureRecognizer ();
			changeName.Tapped += (sender, e) => {
				//SaveNewUserName();
			};
			StackLayout ProfilePicSetting = new StackLayout { 
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.Center,
				Children = {
					new Image{
						Source = "ic_person_pin_grey_500_24dp.png",
						HorizontalOptions = LayoutOptions.Start
					},
					new Label{
						Text = "Change my Profile Pic",
						LineBreakMode = LineBreakMode.WordWrap,
						HorizontalTextAlignment = TextAlignment.Start
					},

				}
			};
			StackLayout UserNameSetting = new StackLayout { 
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.Center,
					Children = {
					new Image{
						Source = "ic_person_grey_500_24dp.png",
						HorizontalOptions = LayoutOptions.Start
					},
					new Label{
						Text = "Change my username",
						LineBreakMode = LineBreakMode.WordWrap,
						HorizontalTextAlignment = TextAlignment.Start
					},

				}
			};

			ProfilePicSetting.GestureRecognizers.Add (changePic);
			UserNameSetting.GestureRecognizers.Add (changeName);

			var stack = new StackLayout{
				Orientation = StackOrientation.Vertical,
				Children = {
					ProfilePicSetting, UserNameSetting
				}
			};

			Debug.WriteLine ("done w settingscell");

			Content = new StackLayout { 
				Orientation = StackOrientation.Vertical,
				BackgroundColor = Color.White,
				Padding = new Thickness(20),
				Children = {
					new Label { Text = "Under Construction" },
					stack
				}
			};
		}


	}
}


