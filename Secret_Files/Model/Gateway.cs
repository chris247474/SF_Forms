using System;
using Newtonsoft.Json;

namespace Secret_Files
{
	public class Gateway
	{
		string id;

		[JsonProperty(PropertyName = "id")]
		public string ID 
		{
			get { return id; }
			set { id = value;}
		}


		[Newtonsoft.Json.JsonProperty("pass")]
		public string Pass {get;set;}
	}
}

