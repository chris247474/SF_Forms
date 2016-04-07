using System;
using Newtonsoft.Json;

namespace Secret_Files
{
	public class CommentItem
	{
		string id, userimage = "http://vignette1.wikia.nocookie.net/gangstamanga/images/5/56/Insert-Photo-Here.jpg";

		[JsonProperty(PropertyName = "id")]
		public string ID 
		{
			get { return id; }  
			set { id = value;}
		}

		[Newtonsoft.Json.JsonProperty("groupID")] 
		public string GroupID {get;set;}

		[Newtonsoft.Json.JsonProperty("postID")] 
		public string PostID {get;set;}

		[Newtonsoft.Json.JsonProperty("userCommenterID")] 
		public string UserCommentID {get;set;}

		[Newtonsoft.Json.JsonProperty("userCommenterName")] 
		public string UserCommentName {get;set;}

		[Newtonsoft.Json.JsonProperty(PropertyName = "userImage")]
		public string UserImage{ 
			get
			{
				return userimage;
			} 
			set
			{ 
				userimage = value;
			}
		}
			
		[Newtonsoft.Json.JsonProperty("commentText")] 
		public string CommentText {get;set;}

		[Newtonsoft.Json.JsonProperty("reactionCount")]  
		public int reactionCount {get;set;} 

		[Microsoft.WindowsAzure.MobileServices.Version]
		public string Version { get; set; }
	}
}

