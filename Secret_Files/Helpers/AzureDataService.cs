// To add offline sync support: add the NuGet package WindowsAzure.MobileServices.SQLiteStore
// to all projects in the solution and uncomment the symbol definition OFFLINE_SYNC_ENABLED
// For Xamarin.iOS, also edit AppDelegate.cs and uncomment the call to SQLitePCL.CurrentPlatform.Init()
// For more information, see: http://go.microsoft.com/fwlink/?LinkId=620342 
//#define OFFLINE_SYNC_ENABLED

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Secret_Files.Helpers;
using System.Net;
using Acr.UserDialogs;

namespace Secret_Files
{
	public partial class AzureDataService
	{
		public MobileServiceClient Client{ get; set;}

		IMobileServiceTable<PostItem> postsTable;
		IMobileServiceTable<GroupItem> groupsTable;
		IMobileServiceTable<AccountItem> accountsTable; 
		IMobileServiceTable<CommentItem> commentTable;
		IMobileServiceTable<Gateway> gateTable;

		public async Task Initialize()
		{
			//Create our client
			this.Client = new MobileServiceClient(
				Values.ApplicationURL);

			this.postsTable = Client.GetTable<PostItem>();
			this.groupsTable = Client.GetTable<GroupItem> ();
			this.accountsTable = Client.GetTable<AccountItem> ();
			this.commentTable = Client.GetTable<CommentItem> ();
			this.gateTable = Client.GetTable<Gateway> ();
		}
		public bool IsOfflineEnabled
		{
			get { return postsTable is Microsoft.WindowsAzure.MobileServices.Sync.IMobileServiceSyncTable<PostItem>; }
		}
		public async Task<List<Gateway>> GetGateway(){
			try
			{
				List<Gateway> items = await gateTable
					.Where(gate => gate.ID != null)
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
		public async Task CreatePass(Gateway item){
			try{
				if (item.ID == null)
				{
					await gateTable.InsertAsync(item);
				}
				else
				{
					await gateTable.UpdateAsync(item);
				}
			}catch(Exception e ){
				Debug.WriteLine ("create pass error: "+e.Message);
			}
		}
		public async Task<List<PostItem>> GetSinglePostByPostID(string postid, bool syncItems = false)
		{
			try
			{
				List<PostItem> items = await postsTable
					.Where(PostItem => PostItem.ID == postid)
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
		public async Task<PostItem> GetSinglePostByPostTextTitleUserIDGroupID(string text, string title, string userID, string groupid, bool syncItems = false)
		{
			try
			{
				UserDialogs.Instance.ShowLoading ();
				List<PostItem> items = await postsTable
					.Where(PostItem => PostItem.Body == text).Where (post => post.Title == title).Where (post => post.UserId == userID).Where (post => post.GroupID == groupid)
					.ToListAsync();
				UserDialogs.Instance.HideLoading ();
				return items.ElementAtOrDefault (0);
				/*var itemArr = items.ToArray ();
				for(int c = 0;c < itemArr.Length;c++){
					if(string.Equals (itemArr[c].Body, text) && string.Equals (itemArr[c].Title, title) && string.Equals (itemArr[c].UserId, userID) && string.Equals (itemArr[c].GroupID, groupid)){
						return itemArr[c];
					}
				}*/

				return null;
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
		public async Task<List<PostItem>> GetPostItemsAsync(bool syncItems = false)
		{
			try
			{
				List<PostItem> items = await postsTable
					.Where(PostItem => PostItem.Title != null)
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
		public async Task<List<PostItem>> GetPostItemsByUserIDAsync(string userId, bool syncItems = false)
		{
			try
			{
				List<PostItem> items = await postsTable
					.Where(PostItem => PostItem.UserId == userId)
					.ToListAsync();

				items.Reverse ();
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
		public async Task<List<CommentItem>> GetCommentsByPostAsync(string postID, bool syncItems = false)
		{
			try
			{
				List<CommentItem> items = await commentTable
					.Where(Comment => Comment.PostID == postID)
					.ToListAsync();

			//	items.Reverse ();
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
		public async Task<List<CommentItem>> GetCommentsByGroupIDAsync(string groupId, bool syncItems = false)
		{
			try
			{
				List<CommentItem> items = await commentTable
					.Where(Comment => Comment.GroupID == groupId)
					.ToListAsync();

				//	items.Reverse ();
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
		public async Task<List<AccountItem>> GetAccountsAsync(bool syncItems = false)
		{
			try
			{
				List<AccountItem> items = await accountsTable
					.Where(x => x.ID != null)
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

		public async Task SaveAccountTaskAsync(AccountItem item)
		{
			try{
				if (item.ID == null)
				{
					await accountsTable.InsertAsync(item);
				}
				else
				{
					await accountsTable.UpdateAsync(item);
				}
			}catch(Exception e ){
				Debug.WriteLine ("New account Save error: "+e.Message);
			}
		}

		public async Task<String> CreateNewAccount(){
			Debug.WriteLine ("Creating new account");
			Random random = new Random ();
			var randomusernamenumber = random.Next ();
			var randomusername = randomusernamenumber.ToString ();

			var user = new AccountItem{password = Values.DEFAULTPASSWORD, username = randomusername };
			await SaveAccountTaskAsync (user);

			return (await GetUserID(user, randomusername));
		}
		public async Task<String> GetUserID(AccountItem item, string randomusername){
			try{
				var accounts = (await GetAccountsAsync ()).ToArray ();
				for(int i = 0;i < accounts.Length;i++){
					if(string.Equals(randomusername, accounts[i].username)){
						return accounts [i].ID;
					}
				}

			}catch(WebException){
				Debug.WriteLine ("UserDialog here, no net connection");//user dialog
				//UserDialogs.Instance.WarnToast ("Your data connection is a bit slow right now", null, 2000);
			}
			catch(TaskCanceledException){
				//UserDialogs.Instance.WarnToast ("Your data connection is a bit slow right now", null, 2000);
			}
			return string.Empty;
		}
		public async Task<AccountItem> GetUserByID(string id){
			try{
				List<AccountItem> accounts = (await GetAccountsAsync ()).Where (user => user.ID == id).ToList();
				return accounts.FirstOrDefault ();

			}catch(WebException){
				Debug.WriteLine ("UserDialog here, no net connection");//user dialog
				//UserDialogs.Instance.WarnToast ("Your data connection is a bit slow right now", null, 2000);
			}
			catch(TaskCanceledException){
				//UserDialogs.Instance.WarnToast ("Your data connection is a bit slow right now", null, 2000);
			}catch(Exception e){
				Debug.WriteLine ("GetUserByID error: "+e.Message);
			}
			return null;
		}
		public async Task<List<PostItem>> GetPostItemsByGroupOrderByPopularAsync(string groupid, bool syncItems = false)
		{
			Debug.WriteLine ("In GetPostItemsByGroupAsync");
			try
			{
				List<PostItem> items = await postsTable
					.Where(PostItem => PostItem.GroupID == groupid).OrderByDescending (PostItem => PostItem.reactionCount )
					.ToListAsync();
				
				Debug.WriteLine ("Done w GetPostItemsByGroupAsync");
				//items.Reverse ();
				//items.OrderBy (PostItem => PostItem.reactionCount);
				return items;
			}
			catch (MobileServiceInvalidOperationException msioe)
			{
				Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
			}
			catch(WebException we){
				//UserDialogs.Instance.WarnToast ("Your data connection is a bit slow right now", null, 2000);
			}
			catch(TaskCanceledException){
				//UserDialogs.Instance.WarnToast ("Your data connection is a bit slow right now", null, 2000);

			}
			catch (Exception e)
			{
				Debug.WriteLine(@"Sync error: {0} - {1}", e.ToString (), e.Message);
			}
			return null;
		}public async Task<List<PostItem>> GetPostItemsByGroupOrderByRecentAsync(string groupid, bool syncItems = false)
		{
			Debug.WriteLine ("In GetPostItemsByGroupAsync");
			try
			{
				List<PostItem> items = await postsTable
					.Where(PostItem => PostItem.GroupID == groupid)//.OrderByDescending (PostItem => PostItem.reactionCount )
					.ToListAsync();

				Debug.WriteLine ("Done w GetPostItemsByGroupAsync");
				items.Reverse ();
				return items;
			}
			catch (MobileServiceInvalidOperationException msioe)
			{
				Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
			}
			catch(WebException we){
				//UserDialogs.Instance.WarnToast ("Your data connection is a bit slow right now", null, 2000);
			}
			catch(TaskCanceledException){
				//UserDialogs.Instance.WarnToast ("Your data connection is a bit slow right now", null, 2000);

			}
			catch (Exception e)
			{
				Debug.WriteLine(@"Sync error: {0} - {1}", e.ToString (), e.Message);
			}
			return null;
		}

		public async Task SavePostItemsTaskAsync(PostItem item)
		{
			try{
				if (item.ID == null)
				{
					await postsTable.InsertAsync(item);
				}
				else
				{
					await postsTable.UpdateAsync(item);
				}
			}catch(Exception e){
				Debug.WriteLine ("SavePostItemsTaskAsync error: "+e.Message);
			}
		}

		public async Task<List<GroupItem>> GetGroupItemsAsync(bool syncItems = false)
		{
			Debug.WriteLine ("GetGroupItemsAsync started");
			try
			{
				List<GroupItem> items = await groupsTable
					.Where(Item => Item.groupName != null)
					.ToListAsync();

				Debug.WriteLine ("GetGroupItemsAsync ending");
				return items;
			}
			catch (MobileServiceInvalidOperationException msioe)
			{
				Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
			}
			catch(WebException){
				Debug.WriteLine ("UserDialog here, no net connection");//user dialog
				UserDialogs.Instance.InfoToast ("Your data connection is a bit slow right now");
			}
			catch(TaskCanceledException){
			//	UserDialogs.Instance.WarnToast ("Your data connection is a bit slow right now", null, 2000);
			}
			catch (Exception e)
			{
				Debug.WriteLine(@"Sync error: {0} - {1}", e.ToString (), e.Message);
			}
			return null;
		}
		public async Task SaveGroupsItemsTaskAsync(GroupItem item)
		{
			try{
				if (item.ID == null)
				{
					await groupsTable.InsertAsync(item);
				}
				else
				{
					await groupsTable.UpdateAsync(item);
				}
			}catch(Exception e ){
				Debug.WriteLine ("GroupItem Save error: "+e.Message);
			}
		}
		public async Task SaveCommentItemsTaskAsync(CommentItem item)
		{
			try{
				if (item.ID == null)
				{
					await commentTable.InsertAsync(item);
				}
				else
				{
					await commentTable.UpdateAsync(item);
				}
			}catch(Exception e ){
				Debug.WriteLine ("CommentItem Save error: "+e.Message);
			}
		}
	}
}
