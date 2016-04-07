using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Secret_Files.Helpers;

namespace Secret_Files
{
	public partial class GroupDataService
	{
		//static GroupDataService defaultInstance = new GroupDataService();
		public MobileServiceClient Client{ get; set;}

		IMobileServiceTable<GroupItem> postsTable;

		public async Task Initialize()
		{
			//from https://blog.xamarin.com/personalized-experiences-with-azure-mobile-apps-authentication/
			var handler = new AuthHandler();

			//Create our client
			this.Client = new MobileServiceClient(
				Values.ApplicationURL, handler);
			handler.Client = this.Client;

			if (!string.IsNullOrWhiteSpace (Settings.AuthToken) && !string.IsNullOrWhiteSpace (Settings.UserId)) {
				this.Client.CurrentUser = new MobileServiceUser (Settings.UserId);
				this.Client.CurrentUser.MobileServiceAuthenticationToken = Settings.AuthToken;
			}
			////from https://blog.xamarin.com/personalized-experiences-with-azure-mobile-apps-authentication/

			this.postsTable = Client.GetTable<GroupItem>();
		}



		public bool IsOfflineEnabled
		{
			get { return postsTable is Microsoft.WindowsAzure.MobileServices.Sync.IMobileServiceSyncTable<GroupItem>; }
		}

		public async Task<List<GroupItem>> GetGroupItemsAsync(bool syncItems = false)
		{
			try
			{
				List<GroupItem> items = await postsTable
					.Where(GroupItem => GroupItem.Title != null)
					.ToListAsync();

				return items;
			}
			catch (MobileServiceInvalidOperationException msioe)
			{
				Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
			}
			catch (Exception e)
			{
				Debug.WriteLine(@"Sync error: {0} - {1}", e.ToString (), e.Message);
			}
			return null;
		}

		public async Task SaveTaskAsync(GroupItem item)
		{
			if (item.ID == null)
			{
				await postsTable.InsertAsync(item);
			}
			else
			{
				await postsTable.UpdateAsync(item);
			}
		}
	}
}

