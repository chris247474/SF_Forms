using System;

using Xamarin.Forms;
using System.Collections.Generic;

namespace Secret_Files
{
	public class AdminView : ContentPage
	{
		ListView listView;
		List<string> list;
		public AdminView ()
		{
			CreateView ();
		}
		async void CreateView(){
			this.Title = "Admins";

			Content = new ScrollView {
				Content = new StackLayout{
					Orientation = StackOrientation.Vertical,
					HorizontalOptions = LayoutOptions.Fill,
					VerticalOptions = LayoutOptions.StartAndExpand,
					BackgroundColor = Color.White,
					Children = { 
						UIBuilder.CreateSeparator (Color.Gray, 0.5),
						UIBuilder.CreateSetting ("", "Add admins", 
							new TapGestureRecognizer{Command = new Command(() =>
								{}
							)}),
						UIBuilder.CreateSeparator (Color.Gray, 0.5),
						UIBuilder.CreateSetting ("", "Send anonymous invites", 
							new TapGestureRecognizer{Command = new Command(() =>
								{
									//send to non app users
									//send to existing users
								}
							)}),
						UIBuilder.CreateSeparator (Color.Gray, 0.5),
						UIBuilder.CreateSetting ("", "Delete", 
							new TapGestureRecognizer{Command = new Command(() =>
								{
									//check how many members first
								}
							)}),
						UIBuilder.CreateSeparator (Color.Gray, 0.5),
					}
				}
			};
		}
	}
}


