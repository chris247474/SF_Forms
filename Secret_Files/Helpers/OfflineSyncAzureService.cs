using System;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using System.Collections.Generic;
using System.Diagnostics;
using Acr.UserDialogs;
using System.Linq;
using System.Net;
using System.Collections.ObjectModel;

namespace Secret_Files
{
	public partial class OfflineSyncAzureService
	{
		public MobileServiceClient Client{ get; set;}

		IMobileServiceTable<PostItem> postsTable;
		IMobileServiceSyncTable<GroupItem> groupsTable;
		IMobileServiceTable<AccountItem> accountsTable; 
		IMobileServiceTable<CommentItem> commentTable;
		IMobileServiceTable<Gateway> gateTable;

		public async Task Initialize()
		{
			//Create our client
			this.Client = new MobileServiceClient(
				Values.ApplicationURL);

			var store = new MobileServiceSQLiteStore("local1.db");
			store.DefineTable<GroupItem>();


			//Initializes the SyncContext using the default IMobileServiceSyncHandler.
			this.Client.SyncContext.InitializeAsync(store);

			this.postsTable = Client.GetTable<PostItem>();
			this.groupsTable = Client.GetSyncTable<GroupItem> ();
			this.accountsTable = Client.GetTable<AccountItem> ();
			this.commentTable = Client.GetTable<CommentItem> ();
			this.gateTable = Client.GetTable<Gateway> ();
		}
		public async Task SyncAsync(bool showloading = false)
		{
			if (showloading)
				UserDialogs.Instance.ShowLoading ();
			ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;
			Debug.WriteLine ("Syncing Azure DB w local");
			try
			{
				await this.Client.SyncContext.PushAsync();

				//The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
				//Use a different query name for each unique query in your program
				await this.groupsTable.PullAsync(
					"allGroups",
					this.groupsTable.CreateQuery());
			}
			catch (MobileServicePushFailedException exc)
			{
				if (exc.PushResult != null)
				{
					syncErrors = exc.PushResult.Errors;
				}
			}
			catch(Exception e){
				UserDialogs.Instance.WarnToast ("Please turn on your data to load more Secrets: "+e.Message);
			}

			// Simple error/conflict handling. A real application would handle the various errors like network conditions,
			// server conflicts and others via the IMobileServiceSyncHandler.
			if (syncErrors != null)
			{
				foreach (var error in syncErrors)
				{
					if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
					{
						//Update failed, reverting to server's copy.
						await error.CancelAndUpdateItemAsync(error.Result);
					}
					else
					{
						// Discard local change.
						await error.CancelAndDiscardItemAsync();
					}

					Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.", error.TableName, error.Item["id"]);
				}
			}

			if (showloading)
				UserDialogs.Instance.HideLoading ();
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
		}public async Task<List<PostItem>> GetPostItemsByGroupOrderByRecentAsync(string groupid, bool showtrending = false, bool syncItems = false)
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
				SyncAsync (true);
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

