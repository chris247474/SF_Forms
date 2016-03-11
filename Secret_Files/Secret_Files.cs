using System;

using Xamarin.Forms;

namespace Secret_Files
{
	public class App : Application
	{
		public static AzureDataService DataDB = new AzureDataService ();
		public App ()
		{
			// The root page of your application
			MainPage = new NavigationPage(new MasterStartPage());
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

