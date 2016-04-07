using System;
using Xamarin.Forms;
using Secret_Files.Helpers;
using Plugin.Media;
using Acr.UserDialogs;
using System.Diagnostics;

namespace Secret_Files
{
	public class CreateNewGroupPage:ContentPage
	{
		ToolbarItem Done, Cancel;
		TapGestureRecognizer tapHandler;
		Entry groupNameEntry;
		public Image img;
		Editor desc;
		GridWithSelectedImageString grid;

		public CreateNewGroupPage ()
		{
			CreateUI ();
		}
		void CreateUI(){
			grid = new GridWithSelectedImageString ();
			UIBuilder.BuildGroupPicGrid (grid, new string[]{"ic_cake_amber_500_24dp.png", "ic_cake_blue_700_24dp.png", "ic_cake_deep_orange_700_24dp.png", "ic_cake_green_600_24dp.png", "ic_cake_pink_400_24dp.png" });//add images
			Debug.WriteLine ("{0} elements in grid", grid.Children.Count);

			this.BackgroundColor = Color.White;

			img = new Image{
				Source = "ic_camera_alt_grey_500_24dp.png",
				HorizontalOptions = LayoutOptions.CenterAndExpand,
			};

			tapHandler = new TapGestureRecognizer{
				NumberOfTapsRequired = 1
			};
			tapHandler.Tapped += async (sender, e) => {
				var file = await App.Camera.ChooseImage (this);
				if(!string.IsNullOrEmpty (file)){
					img.Source = file;
					//upload to cloud
				}
			};
			img.GestureRecognizers.Add (tapHandler);

			desc = new Editor{ 
				Text = "Enter something to tell your followers what this secret file is for...",
				HorizontalOptions = LayoutOptions.CenterAndExpand
				//TextColor = Color.Silver
			};
			desc.Focused += (sender, e) => {
				if(string.Equals (desc.Text, "Enter something to tell your followers what this secret file is for...")){
					desc.Text = "";
				}
			};
			desc.Unfocused += (sender, e) => {
				if(string.IsNullOrWhiteSpace (desc.Text)){
					desc.Text = "Enter something to tell your followers what this secret file is for...";
				}
			};

			groupNameEntry = new Entry {
				Text = "What do you want to call this secret file?",
				TextColor = Color.Silver,
				Keyboard = Keyboard.Create(KeyboardFlags.All),
				HorizontalOptions = LayoutOptions.CenterAndExpand,
			};
			groupNameEntry.Focused += (sender, e) => {
				if(string.Equals (groupNameEntry.Text, "What do you want to call this secret file?")){
					groupNameEntry.Text = "";
				}
				groupNameEntry.TextColor = Color.Default;
			};
			groupNameEntry.Unfocused += (sender, e) => {
				if(string.IsNullOrWhiteSpace (groupNameEntry.Text)){
					groupNameEntry.Text = "What do you want to call this secret file?";
					groupNameEntry.TextColor = Color.Silver;
				}
			};
			groupNameEntry.Focus ();

			Done = new ToolbarItem ("Done", "ic_done_white_24dp.png", async () => {
				//save to table
				if(string.IsNullOrWhiteSpace (groupNameEntry.Text)){
					UserDialogs.Instance.WarnToast ("Oops! Can't make a secret file without a name!", null, 2000);
				}else if(string.IsNullOrWhiteSpace (grid.StringImageSelected)){
					UserDialogs.Instance.WarnToast ("Please don't forget to choose a secret file picture", null, 2000);
				}else{
					if(!RegexHelper.MatchesOfficialSecretFilesThreadName (groupNameEntry.Text)){
						await App.DataDB.SaveGroupsItemsTaskAsync (new GroupItem{groupName = groupNameEntry.Text, 
							groupDesc = desc.Text, adminuserID = Settings.UserId, groupImage = grid.StringImageSelected});
						Util.NewGroupNotif (this);
						await Navigation.PopAsync ();
					}else{
						await this.DisplayAlert ("A Secret File with that name already exists", "To avoid confusion we don't allow new threads named 'Secret Files' or anything similar. Please think of a different name", "OK");
					}
				}
			});
			Cancel = new ToolbarItem ("Cancel", "ic_clear_white_24dp.png", () => {
				Navigation.PopAsync ();
			});
			this.ToolbarItems.Add (Done);
			//this.ToolbarItems.Add (Cancel);

			Content = new StackLayout { 
				Orientation = StackOrientation.Vertical, 
				Padding = new Thickness(20),
				Children = { 
					//img, no option to upload custom pics yet. minimize server costs
					new StackLayout{
						Orientation = StackOrientation.Vertical,
						Children = {
							new Label{
								Text = "Choose a Secret Files Pic",
								HorizontalTextAlignment = TextAlignment.Center,
								HorizontalOptions = LayoutOptions.Center,
								FontSize = Device.GetNamedSize (NamedSize.Large, typeof(Label))
							},
							/*new Label{
								Text = "To keep Secret Files and its community anonymous, we don't allow users to upload profile pictures. So have fun choosing your new face!",
								HorizontalTextAlignment = TextAlignment.Center,
								HorizontalOptions = LayoutOptions.Center,
								FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label))
							},*/
							UIBuilder.CreateSeparatorWithTopBottomSpacing (Color.Gray, 0.5),
							grid,
							UIBuilder.CreateWhiteSpacing (10),
						}
					},
					new StackLayout {
						Orientation = StackOrientation.Horizontal,
						HorizontalOptions = LayoutOptions.Center,
						Children = { groupNameEntry}
					}, desc
				}
			};
		}
	}
}

