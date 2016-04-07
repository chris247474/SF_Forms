using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Secret_Files
{
	public static class ChatMessageService
	{
		public const string DELIMITER = "_delimiter_";
		public static string FormatMessage(string body, string username, string groupid, string userid, string postimage, string groupname){
			var texttosend = body+DELIMITER+username+DELIMITER+groupid+DELIMITER+userid+DELIMITER+postimage+DELIMITER+groupname; 
			Debug.WriteLine ("formatted string is {0}", texttosend);
			return texttosend;
		}
		public async static Task<PostItemStackLayout> UnformatMessage(string message){
			if(string.IsNullOrWhiteSpace (message)){
				return null;
			}

			string[] messageValues = message.Split (new[] { DELIMITER }, StringSplitOptions.RemoveEmptyEntries);

			Debug.WriteLine ("received post:");
			Debug.WriteLine ("body: {0}", messageValues[0]);
			Debug.WriteLine ("username: {0}", messageValues[1]);
			Debug.WriteLine ("groupid: {0}", messageValues[2]);
			Debug.WriteLine ("userid: {0}", messageValues[3]);
			Debug.WriteLine ("postimage: {0}", messageValues[4]);
			Debug.WriteLine ("groupname: {0}", messageValues[5]);

			return new PostItemStackLayout (null);
		}
	}
}

