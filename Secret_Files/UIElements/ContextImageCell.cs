using System;
using Xamarin.Forms;
using Secret_Files.Helpers;
using System.Diagnostics;
using Acr.UserDialogs;

namespace Secret_Files
{
	public class ContextImageCell:ImageCell
	{
		bool IsAdmin;
		MenuItem EditName, DeleteThread, AddAdmin;//admin
		MenuItem LeaveThread, InviteFriends, SettingsMI;//normal user
		GroupItem GroupSelected = null;
		GroupsPage Page;

		public ContextImageCell (GroupsPage page):base()
		{
			this.Page = page;
			SettingsMI = new MenuItem{Text = "Settings", Icon = "ic_settings_grey_500_24dp.png" };
			SettingsMI.SetBinding (MenuItem.CommandParameterProperty, new Binding ("."));
			SettingsMI.Clicked += (sender, e) => {
				var mi = ((MenuItem)sender);
				GroupSelected = (GroupItem)mi.BindingContext;
				Debug.WriteLine ("Settings tapped, adminid {0}", GroupSelected.adminuserID);
				CheckIfAdminThenAdaptOptions(GroupSelected.adminuserID);
			};

			InviteFriends = new MenuItem{Text = "Invite"};
			InviteFriends.SetBinding (MenuItem.CommandParameterProperty, new Binding ("."));
			InviteFriends.Clicked += (sender, e) => {
				var mi = ((MenuItem)sender);
				GroupSelected = (GroupItem)mi.BindingContext;
				UserDialogs.Instance.WarnToast ("Under construction");
			};

			LeaveThread = new MenuItem{Text = "Leave"};
			LeaveThread.SetBinding (MenuItem.CommandParameterProperty, new Binding ("."));
			LeaveThread.Clicked += (sender, e) => {
				var mi = ((MenuItem)sender);
				GroupSelected = (GroupItem)mi.BindingContext;
				UserDialogs.Instance.Confirm (new ConfirmConfig{
					OnConfirm = (bool obj) => {
						UserDialogs.Instance.WarnToast ("Under construction");
					},
					Title = "This would be unwise...",
					CancelText = "Keep me in the loop",
					OkText = "Yup",
					Message = "Are you sure you want to leave this Secret Thread? To rejoin, someone will have to invite you all over again..."
				});
			};

			ContextActions.Add (InviteFriends);
			//ContextActions.Add (LeaveThread);
			ContextActions.Add (SettingsMI);
		}
		void CheckIfAdminThenAdaptOptions(string adminid){
			if (string.Equals (Settings.UserId, adminid)) {
				Debug.WriteLine ("User {0} is admin {1}", Settings.UserId, adminid);
				Page.Navigation.PushAsync (new AdminView());
			} else {
				Debug.WriteLine ("User {0} not admin {1}", Settings.UserId, adminid);
				UserDialogs.Instance.WarnToast ("Sorry, only a Secret Thread Admin can access this. Ask an admin to make you one too!");
			}
		}
		void ClearMIAdmin(){
			/*ContextActions.Clear ();
				ContextActions.Add (EditName);
				ContextActions.Add (DeleteThread);
				ContextActions.Add (AddAdmin);
				ContextActions.Add (SettingsMI);*/
		}
		void ClearMIUser(){
			/*ContextActions.Clear ();
			ContextActions.Add (InviteFriends);
			ContextActions.Add (LeaveThread);
			ContextActions.Add (SettingsMI);*/
		}
	}
}

