using System;
using Xamarin.Forms;
using System.Diagnostics;
using System.Threading.Tasks;
using Secret_Files.Helpers;

namespace Secret_Files
{
	public static class UIBuilder
	{
		public static void ReplaceTransparentCommentOrAdd(StackLayout container, CommentItem message, PostItemStackLayout parentPost, PostItem post){
			View[] view = new View[container.Children.Count];
			container.Children.CopyTo (view, 0);
			Debug.WriteLine ("{0} comments in stack", view.Length);

			for(int c = 6;c < view.Length;c++){
				try{
					//Debug.WriteLine ("View {0} is {1}", c, view[c].GetType ());
					if(view[c].GetType () == typeof(CommentStackLayout)){
						Debug.WriteLine ("Checking commenter {0} at index {1}", (view [c] as CommentStackLayout).UserCommenter, c);
						if(string.Equals ((view[c] as CommentStackLayout).Text, message.CommentText) && string.Equals ((view[c] as CommentStackLayout).UserCommenter, message.UserCommentName) && (view[c] as CommentStackLayout).Opacity == 0.3){
							Debug.WriteLine ("user has a pending comment at index {0}, solidifying it", c);
							container.Children.RemoveAt (c);
							container.Children.Insert (c, new CommentStackLayout (parentPost, post, Settings.ProfilePic, Settings.Username, message.CommentText));
						}
					}
				}catch(Exception){
					Debug.WriteLine ("not a comment");
				}
			}
		}
		public static StackLayout ComposeInfoPageStack(){
			return new StackLayout{
				Orientation = StackOrientation.Vertical,
				Padding = new Thickness(20),
				Children = {
					ComposeParagraph("", ""),

				}
			};
		}
		public static StackLayout ComposeParagraph(string headerStr, string bodyStr){
			Label headerLabel = new Label{ 
				Text = headerStr,
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.Center,
				FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label))
			};
			Label bodyLabel = new Label{ 
				Text = bodyStr,
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.Center,
				FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label))
			};

			return new StackLayout{ 
				Orientation = StackOrientation.Vertical,
				//Padding = new Thickness(20),
				Children = {
					headerLabel,
					//spacing?
					bodyLabel,
				}
			};
		}
		public static Image CreateTappableImage(string source, Command TapAction = null){
			Image img = new Image{
				Aspect = Aspect.AspectFit,
				Source = source
			};
			if (TapAction != null) {
				img.GestureRecognizers.Add (new TapGestureRecognizer{ Command = TapAction});
			} else {
				img.GestureRecognizers.Add (new TapGestureRecognizer{ Command = new Command (() => Util.ChangeProfilePic (img)) });
			}
			return img;
		}
		public static Image CreateTappableGroupImage(GridWithSelectedImageString grid, string source, Command TapAction = null){
			Image img = new Image{
				Aspect = Aspect.AspectFit,
				Source = source
			};
			if (TapAction != null) {
				img.GestureRecognizers.Add (new TapGestureRecognizer{ Command = TapAction});
			} else {
				img.GestureRecognizers.Add (new TapGestureRecognizer{ Command = new Command (() => Util.ChangeGroupPic (grid, img)) });
			}
			return img;
		}

		public static void BuildProfilePicGrid(GridWithSelectedImageString grid, string[] pics){
			if(grid == null){
				return;
			}

			if (pics.Length <= 0 || pics == null) {
				return;
			}

			for(int c = 0;c < pics.Length;c++){
				grid.Children.AddHorizontal (UIBuilder.CreateTappableImage (pics [c]));
			}
		}
		public static void BuildGroupPicGrid(GridWithSelectedImageString grid, string[] pics){
			if(grid == null){
				return;
			}

			if (pics.Length <= 0 || pics == null) {
				return;
			}

			for(int c = 0;c < pics.Length;c++){
				grid.Children.AddHorizontal (UIBuilder.CreateTappableGroupImage (grid, pics [c]));
			}
		}

		public static StackLayout CreateSetting(string icon, string name, TapGestureRecognizer handler){
			
			StackLayout Setting = new StackLayout { 
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.Start,
				Padding = new Thickness(20),
				Children = {
					new Image{
						Source = icon,
						HorizontalOptions = LayoutOptions.Start
					},
					new Label{
						Text = name,
						LineBreakMode = LineBreakMode.WordWrap,
						HorizontalTextAlignment = TextAlignment.Center
					},

				}
			};
			Setting.GestureRecognizers.Add (handler);
			return Setting;
		}
		public static BoxView CreateSeparator(Color borderColor, double opacity){
			return new BoxView () { Color = Color.Gray, HeightRequest = 1, Opacity = opacity  };
		}
		public static StackLayout CreateSeparatorWithBottomSpacing(Color borderColor, double opacity, double bottomspacing = 5){
			return new StackLayout{ 
				Orientation = StackOrientation.Vertical,
				Children = {
					new BoxView() { Color = Color.Gray, HeightRequest = 1, Opacity = 0.5},
					new BoxView() { Color = Color.White, HeightRequest = bottomspacing  },
				}
			};
		}
		public static StackLayout CreateSeparatorWithTopSpacing(Color borderColor, double opacity, double topspacing = 5){
			return new StackLayout{ 
				Orientation = StackOrientation.Vertical,
				Children = {
					new BoxView() { Color = Color.White, HeightRequest = topspacing  },
					new BoxView() { Color = Color.Gray, HeightRequest = 1, Opacity = 0.5},
				}
			};
		}
		public static StackLayout CreateSeparatorWithTopBottomSpacing(Color borderColor, double opacity, double topbottomspacing = 5){
			return new StackLayout{ 
				Orientation = StackOrientation.Vertical,
				Children = {
					new BoxView() { Color = Color.White, HeightRequest = topbottomspacing  },
					new BoxView() { Color = Color.Gray, HeightRequest = 1, Opacity = 0.5},
					new BoxView() { Color = Color.White, HeightRequest = topbottomspacing  },
				}
			};
		}
		public static BoxView CreateWhiteSpacing(double spacing = 5){
			return new BoxView () { Color = Color.White, HeightRequest = spacing  };
		}
	}
}

