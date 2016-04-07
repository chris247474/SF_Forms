using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Refractored.XamForms.PullToRefresh.Droid;
using ImageCircle.Forms.Plugin.Droid;
using Acr.UserDialogs;

namespace Secret_Files.Droid
{
	[Activity (Label = "Secret Files", Icon = "@drawable/icon", /*MainLauncher = true,*/ ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, 
		Theme = "@style/HideIcon")]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			Xamarin.Insights.Initialize (global::Secret_Files.Droid.XamarinInsights.ApiKey, this);
			Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();
			PullToRefreshLayoutRenderer.Init ();
			//Xamarin.Forms.Init();//platform specific init
			ImageCircleRenderer.Init();
			base.OnCreate (bundle);
			global::Xamarin.Forms.Forms.Init (this, bundle);

			//init acr userdialogs
			UserDialogs.Init(this);

			LoadApplication (new App ());
		}
	}
}
