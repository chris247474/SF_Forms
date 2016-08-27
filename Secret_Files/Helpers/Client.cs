using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using System.Diagnostics;
using Microsoft.AspNet.SignalR.Client.Transports;
using Xamarin.Forms;
using Secret_Files.Helpers;
using System.Collections.Generic;

namespace Secret_Files
{
	public class Client
	{
		private readonly string _platform;
		private readonly HubConnection _connection;
		private readonly IHubProxy _proxy;

		public event EventHandler<PostItem> OnMessageReceived;
		public event EventHandler<CommentItem> OnCommentReceived;
		public event EventHandler<string> OnBroadcastMessageReceived;

		public Client(string platform)
		{
			_platform = platform;
			_connection = new HubConnection("http://chrisdavetvsignalr.azurewebsites.net/");
			_proxy = _connection.CreateHubProxy("ChatHub");
		}

		public async Task Connect()
		{
			Debug.WriteLine ("Connecting");
			await _connection.Start ();
			Debug.WriteLine ("Connected");

			_proxy.On("messageReceived", (PostItem message) =>
				{
					if (OnMessageReceived != null)
						OnMessageReceived(this, message);
				});

			_proxy.On("commentReceived", (CommentItem message) =>
				{
					if (OnCommentReceived != null)
						OnCommentReceived(this, message);
				});

			_proxy.On("broadcastMessage", (string name, string message) =>
				{
					if (OnBroadcastMessageReceived != null)
						OnBroadcastMessageReceived(this, string.Format("{0}: {1}", name, message));
				});
		}

		public Task Send(PostItem message)
		{
			return _proxy.Invoke("SendToSecretFilesClient", message);
		}
		public Task SendComment(CommentItem message)
		{
			return _proxy.Invoke("SendComentsToClients", message);
		}
		public Task SendToWeb(string name, string message)
		{
			return _proxy.Invoke("Send", name, message);
		}

		public async Task ListenForNewPosts(PullToRefreshViewModel refreshModel, StackLayout PostsContainer, string groupid){
			App.ChatClient.OnMessageReceived += (sender, message) => {
				Debug.WriteLine ("Post data received, processing");
				//add data to new postitemstacklayout, add to top of scrollfeed.content
				if(message is PostItem){
					Debug.WriteLine ("Recieved from SignalR: post by {0}, post body: {1}, in group {2}", message.Title, message.Body, message.GroupID);
					UpdatePosts (refreshModel, PostsContainer, message, groupid);
				}
			};
		}

		void UpdatePosts(PullToRefreshViewModel refreshModel, StackLayout PostsContainer, PostItem message, string groupid){
			if(string.Equals (message.GroupID, groupid)){
				Debug.WriteLine ("signalr message and current group match!");
				//PostItem postwID = await App.DataDB.GetSinglePostByPostTextTitleUserIDGroupID(message.Body, message.Title, message.UserId, message.GroupID);//dont delete, sometimes azure gets delayed so calling it here gives it time
				if(message != null){
					Debug.WriteLine ("Recieved PostItem, generated ID is: {0}", message.ID);

					//add to top stack
					Device.BeginInvokeOnMainThread (() => {
						if(PostsContainer == null){
							Debug.WriteLine ("no posts in feed yet");
							refreshModel.ExecuteRefreshCommand ();
						}else{
							Debug.WriteLine ("{0}, posts in feed", PostsContainer.Children.Count);
							PostsContainer.Children.Insert (1, new PostItemStackLayout(message));
						}
					});
				}else{
					Debug.WriteLine ("Couldn't fetch PostItem from db");
				}

			}else{
				Debug.WriteLine ("PostItem {0} received, does not match group id {1}", message.ID, message.GroupID);
			}
		}
		public async Task ListenForNewComments(PostItemStackLayout parentPost, StackLayout container, PostItem post){
			App.ChatClient.OnCommentReceived += async (sender, message) => {
				if(message is CommentItem){
					Debug.WriteLine ("Recieved Comment from SignalR: comment by {0}, comment body: {1}, in group {2}, in post {3}", message.UserCommentName, message.CommentText, message.GroupID, message.PostID);

					if(string.Equals (message.PostID, post.ID)){
						Debug.WriteLine ("Found post where comment was typed, adding it in");
						//post comment
						Device.BeginInvokeOnMainThread (() => {

							UIBuilder.ReplaceTransparentCommentOrAdd(container, message, parentPost, post);
						});
					}
				}
			};
		}

	}
}

