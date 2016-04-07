using System;

using Xamarin.Forms;
using Secret_Files.Helpers;
using Microsoft.WindowsAzure.MobileServices;
using System.Diagnostics;
using System.Threading.Tasks;
using Plugin.Media;
using Acr.UserDialogs;

namespace Secret_Files
{
	public class App : Application
	{
		public static AzureDataService DataDB = new AzureDataService ();
		public static string NewestOrTrending = Values.NEWEST;
		public static ContentPage groupfeed;
		public static CameraService Camera = new CameraService ();
		public static Client ChatClient;
		public static NavigationPage NavPage;

		public App ()
		{
			for(int c = 0;c < 20;c++){
				Debug.WriteLine ("random {0}: {1}",c,Util.GenerateRandomUsername ());
			}
			PrepareAppData ();
			//CheckIfAppAllowedThenLoadMainPage ();
			NavPage = new NavigationPage (new MasterStartPage ()) {
				BarBackgroundColor = Color.FromHex (Values.TESTCOLOR),
				BarTextColor = Color.FromHex (Values.BACKGROUNDLIGHTSILVER) 
			};
			MainPage = NavPage;
		}
		async Task ConnectChatClientToServer(){
			if (Device.OS == TargetPlatform.Android) {
				Debug.WriteLine ("initializing SignalR for Android");
				ChatClient = new Client ("Android");
			}else if(Device.OS == TargetPlatform.iOS){
				ChatClient = new Client ("iOS");
			}

			try{
				await ChatClient.Connect ();


				ChatClient.OnBroadcastMessageReceived += (sender, message) => {
					Debug.WriteLine ("Recieved from broadcastMessage: {0}", message);
					//SignalRReply(message, true);
				};

				//Debug.WriteLine ("Sending message through signalr");
				//ChatClient.Send("Test Message");
			}catch(Exception e){
				Debug.WriteLine ("Could not connect to SignalR server: {0} - {1}", e.InnerException, e.Message);
				try{
					await ChatClient.Connect ();
				}catch(Exception ex){
					UserDialogs.Instance.WarnToast ("Can't connect to Secret Files", "Your connection isn't so great right now. Try quitting and reopening the app", 2000);
				}
			}
		}
		async void SignalRReply(string message, bool web){
			var result = await UserDialogs.Instance.PromptAsync(message, "Reply", "Ok", "Cancel");
			if(result.Ok){
				if (string.Equals (result.Text, "stop")) {
					return;
				} else {
					if (web)
						ChatClient.SendToWeb ("android", result.Text);
					//else
						//ChatClient.Send (result.Text);
				}
			}
		}
		async void CheckIfAppAllowedThenLoadMainPage(){
			Debug.WriteLine ("Checking killswitch status");
			var x = await DataDB.GetGateway ();
			if (x.Count > 0 || x != null) {
				MainPage = new NavigationPage (new ContentPage{Content = new StackLayout{Children = {new Label{Text = "Your app is outdated"}}}}) {
					BarBackgroundColor = Color.FromHex (Values.TESTCOLOR),
					BarTextColor = Color.FromHex (Values.BACKGROUNDLIGHTSILVER)
				};
			} else {
				MainPage = new NavigationPage (new MasterStartPage ()) {
					BarBackgroundColor = Color.FromHex (Values.TESTCOLOR),
					BarTextColor = Color.FromHex (Values.BACKGROUNDLIGHTSILVER) 
				};
			}
		}
		async void PrepareAppData(){
			ConnectChatClientToServer ();//move to GroupFeed?
			await App.DataDB.Initialize();
			await CheckIfLoggedInThenContinue ();

			//await SetupKillSwitch (Values.UNIVPASS);
		}
		async Task SetupKillSwitch(string NewPass){
			Values.UNIVPASS = NewPass;
			await DataDB.CreatePass (new Gateway{Pass = Values.UNIVPASS});
		}
		async Task CheckIfLoggedInThenContinue(){
			Debug.WriteLine ("Entered CheckIfLoggedIn");
			if (!Settings.IsLoggedIn) {
				Settings.UserId = await App.DataDB.CreateNewAccount ();
				Settings.Username = Util.GenerateRandomUsername ();
				if (string.IsNullOrWhiteSpace (Settings.UserId)) {
					Debug.WriteLine ("ID was not saved into app settings");
				} else if (string.IsNullOrWhiteSpace (Settings.Username)) {
					Debug.WriteLine ("Username was not saved into app settings");
				}{
					Debug.WriteLine ("new account {0} name {1}", Settings.UserId, Settings.Username);
				}
				//pull latest data from server:
				Util.LoggedInNotif (this);
			} else {
				Debug.WriteLine ("User already logged in");
			}
			Debug.WriteLine ("Done CheckIfLoggedIn");
		}
		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

