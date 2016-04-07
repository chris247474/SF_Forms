using System;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using Secret_Files.Helpers;
using Xamarin.Forms;
using System.Diagnostics;
using Secret_Files.Droid;

[assembly:Dependency(typeof(Authentication))]
namespace Secret_Files.Droid
{
	public class Authentication: IAuthentication
	{
		public async Task<MobileServiceUser> LoginAsync(MobileServiceClient client, MobileServiceAuthenticationProvider provider)
		{
			Debug.WriteLine ("Entered LoginASync");
			try
			{
				//login and save user status
				var user = await client.LoginAsync(Forms.Context, provider);
				Settings.AuthToken = user?.MobileServiceAuthenticationToken ?? string.Empty;
				Settings.UserId = user?.UserId ?? string.Empty;
				return user;
			}
			catch(Exception e)
			{
				e.Data["method"] = "LoginAsync";
				Xamarin.Insights.Report(e);
			}

			Debug.WriteLine ("Returning null");
			return null;
		}
	}
}

