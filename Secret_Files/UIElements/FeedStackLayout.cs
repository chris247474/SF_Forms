using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Secret_Files
{
	public class FeedStackLayout:StackLayout
	{
		string groupid;
		public FeedStackLayout (List<PostItemStackLayout> list, string groupid):base()
		{
			this.groupid = groupid;

			Orientation = StackOrientation.Vertical;
			this.Children.Add ( 
				new StackLayout{
					BackgroundColor = Color.White,
					Children = {Util.CreateSearchBar()}
				}
			);

			if (list == null) {
				//UserDialogs.Instance.InfoToast ("No posts here yet", null, 2000);//change to text background?
			} else {
				var listArr = list.ToArray (); 

				for(int c = 0;c < listArr.Length;c++){
					this.Children.Add (listArr[c]); 
				}
			}
		}

	}
}

