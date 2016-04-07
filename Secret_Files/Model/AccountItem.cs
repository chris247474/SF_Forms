using System;
using Newtonsoft.Json;

namespace Secret_Files
{
	public class AccountItem
	{
		string id;

		[JsonProperty(PropertyName = "id")]
		public string ID 
		{
			get { return id; }
			set { id = value;}
		}

		[JsonProperty("username")]
		public string username { get; set;}

		[JsonProperty("password")]
		public string password { get; set;}

		[Microsoft.WindowsAzure.MobileServices.Version]
		public string Version { get; set; }
	}
}

