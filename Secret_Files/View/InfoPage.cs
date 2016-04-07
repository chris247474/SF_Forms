using System;
using Xamarin.Forms;

namespace Secret_Files
{
	public class InfoPage:ContentPage
	{
		public string TextTitle{ get; set;}

		public InfoPage (string text)
		{
			this.BackgroundColor = Color.White;
			ScrollView scrollView;
			Label privacyLabel;

			privacyLabel = new Label{ 
				Text = text,
				LineBreakMode = LineBreakMode.WordWrap,
				FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label))
			};

			scrollView = new ScrollView{ 
				Content = privacyLabel
			};

			Content = new StackLayout{ 
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Padding = new Thickness(30, 20, 20, 20),
				Children = {scrollView}
			};
		}
	}
}

