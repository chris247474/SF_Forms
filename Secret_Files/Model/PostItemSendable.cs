using System;

namespace Secret_Files
{
	public class PostItemSendable
	{
		public string ID { get; set;}
		public string PostImage {get;set;}
		public string UserId {get;set;}
		public string GroupID {get;set;}
		public int reactionCount {get;set;} = 0;
		public string Body { get; set;}
		public string Title{ get; set;}
	}
}

