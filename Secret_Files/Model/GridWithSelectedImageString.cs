using System;
using Xamarin.Forms;

namespace Secret_Files
{
	public class GridWithSelectedImageString:Grid
	{
		public string StringImageSelected{ get; set;}
		public GridWithSelectedImageString ():base()
		{
			ColumnSpacing = 5;
			VerticalOptions = LayoutOptions.Center;
		}
	}
}

