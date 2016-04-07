using System;
using Newtonsoft.Json;

namespace Secret_Files
{
	public class GroupItem
	{
		string id, groupname, adminduserid, groupdesc, groupimage;

		[JsonProperty(PropertyName = "id")]
		public string ID 
		{
			get { return id; }
			set { id = value;}
		}

		[JsonProperty(PropertyName = "groupName")]
		public string groupName 
		{
			get { return groupname; } 
			set { groupname = value;} 
		}

		[JsonProperty(PropertyName = "groupDesc")]
		public string groupDesc 
		{
			get { return groupdesc; } 
			set { groupdesc = value;} 
		}	

		[JsonProperty(PropertyName = "groupImage")]
		public string groupImage 
		{
			get { return groupimage; } 
			set { groupimage = value;} 
		}	

		[JsonProperty("adminuserId")]
		public string adminuserID {get{return adminduserid; }set{adminduserid = value; }}

		[Microsoft.WindowsAzure.MobileServices.Version]
		public string Version { get; set; }
	}
}

