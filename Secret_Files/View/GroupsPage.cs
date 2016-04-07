using System;

using Xamarin.Forms;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using ImageCircle.Forms.Plugin.Abstractions;
using System.Linq;
using System.Collections.Generic;
using Secret_Files.Helpers;
using Acr.UserDialogs;

namespace Secret_Files
{
	public class GroupsPage : ContentPage
	{
		ListView listView;
		ToolbarItem CreateGroup;
		public GroupItem GroupSelected = new GroupItem();
		DataTemplate groupCell;
		List<GroupItem> TempSearchItems = new List<GroupItem>();
		SearchBar searchBar;

		public GroupsPage ()
		{
			CreateView ();
		}
		void SubscribeToRefreshListener(){
			MessagingCenter.Subscribe<CreateNewGroupPage> (this, Values.REFRESH, (args) => { 
				Debug.WriteLine ("REFRESH MESSAGE RECEIVED");
				ExecuteRefreshCommand();
			});
			MessagingCenter.Subscribe<App> (this, Values.REFRESH, (args) => { 
				Debug.WriteLine ("REFRESH MESSAGE RECEIVED");
				ExecuteRefreshCommand();
			});
		}
		async Task refresh(){
			listView.ItemsSource = await App.DataDB.GetGroupItemsAsync ();
		}
		private Command refreshcommand;
		public Command RefreshCommand{
			get{
				return refreshcommand ?? (refreshcommand = new Command(ExecuteRefreshCommand, () => {
					return !IsBusy;
				}));
			}
		}
		private async void ExecuteRefreshCommand(){
			try{
				if (IsBusy) return; 

				IsBusy = true; 
				RefreshCommand.ChangeCanExecute (); 
				//listView.BeginRefresh ();

				TempSearchItems = await App.DataDB.GetGroupItemsAsync ();
				listView.ItemsSource = Util.AssignDefaultPicsToEmptyGroupPics(TempSearchItems);

				listView.EndRefresh ();//usually comes after a listview.BeginRefresh() but for some reason, this combo w IsBusy and ChangeCanExecute works as expected
				IsBusy = false; 
				RefreshCommand.ChangeCanExecute (); 
			}catch(Exception e){
				Debug.WriteLine ("Error in GroupsPage.ExecuteRefreshCommand() ",e.Message);
			}
		}
		async void CreateView(){
			this.BackgroundColor = Color.White;
			Title = "Groups";
			SubscribeToRefreshListener ();

			CreateGroup = new ToolbarItem ("Add", "ic_group_add_white_24dp.png", async () => {
				await Navigation.PushAsync (new CreateNewGroupPage());
			});
			this.ToolbarItems.Add (CreateGroup);

			//setup list of groups that allows for pull-to-refresh functionality
			listView = new ListView{
				//ItemsSource = await App.DataDB.GetGroupItemsAsync ()
			};
			listView.IsPullToRefreshEnabled = true;
			listView.RefreshCommand = RefreshCommand;
			listView.SetBinding (ListView.IsRefreshingProperty, new Xamarin.Forms.Binding(){Path="IsBusy", Mode = BindingMode.OneWay});
			//IsBusy = false;

			groupCell = new DataTemplate (() => {
				return new ContextImageCell (this);
			});
			groupCell.SetBinding (ContextImageCell.TextProperty, new Xamarin.Forms.Binding(){Path="groupName"});
			groupCell.SetBinding (ContextImageCell.DetailProperty, new Xamarin.Forms.Binding(){Path="groupDesc"});
			groupCell.SetBinding (ContextImageCell.ImageSourceProperty, new Xamarin.Forms.Binding(){Path="groupImage"});
			listView.ItemTemplate = groupCell;

			listView.ItemSelected += async (sender, e) => {
				if (e.SelectedItem == null)
					return; 
				
				GroupSelected = (GroupItem)e.SelectedItem;
				await Navigation.PushAsync (new GroupFeed(GroupSelected.groupName, GroupSelected.ID));

				((ListView)sender).SelectedItem = null; 
			};

			searchBar = Util.CreateSearchBar (this);
			Content = new StackLayout { 
				Orientation = StackOrientation.Vertical,
				Padding = new Thickness (0, 0, 0, 15),
				Children = {
					Util.CreateSeparator(Color.Gray, 1),
					searchBar,
					listView
				}
			};

			ExecuteRefreshCommand ();
		}
		public void SearchGroups()
		{
			//listView.BeginRefresh ();

			if (string.IsNullOrWhiteSpace (searchBar.Text)) {
				listView.ItemsSource = TempSearchItems;
			} else {
				listView.ItemsSource = TempSearchItems
					.Where (x => x.groupName.ToLower ().Contains (searchBar.Text.ToLower () ) );
				//listView.ItemTemplate = groupCell;
			}

			//listView.EndRefresh ();
		}
	}
}


