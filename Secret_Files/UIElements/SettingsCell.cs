using System;
using Xamarin.Forms;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using System.Threading.Tasks;

namespace Secret_Files
{
	public class SettingsCell:StackLayout, INotifyPropertyChanged
	{
		public Image SettingsImage;
		public Label TextBody;
		string text, image;
		//TapGestureRecognizer tapHandler;
		//public Action<string> Tapped;

		public SettingsCell (/*Action<string> func*/)//:base()
		{
			/*Tapped = func;
			if(Tapped == null){
				throw new NullReferenceException ("SettingsCell TapGestureRecognizer delegate method 'tapped' must be defined before using");
			}
*/
			CreateView ();
		}
	
		void CreateView(){

			this.Children.Add (
				new StackLayout{
					Orientation = StackOrientation.Horizontal,
					Children = {SettingsImage, TextBody}
				}
			);
		}


		public string Text
		{
			get { return text; }
			set
			{
				Debug.WriteLine ("text");

				text = value;
				OnPropertyChanged("Text");
			}
		}
		private ICommand newtextCommand;
		public ICommand NewTextCommand{
			get { 
				Debug.WriteLine ("newtextCommand");
				return newtextCommand ?? (newtextCommand = new Command(async () => await ExecuteNewTextCommand())); 
			}
		}

		public async Task ExecuteNewTextCommand(){
			Debug.WriteLine ("Started ExecuteNewTextCommand");
			TextBody.Text = text;
		}
		#region INotifyPropertyChanged implementation

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		public void OnPropertyChanged(string propertyName)
		{
			Debug.WriteLine ("Property changed");
			if (PropertyChanged == null)
				return;

			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		public string Image
		{
			get { return image; }
			set
			{
				Debug.WriteLine ("image");

				image = value;
				OnPropertyChanged("Image");
			}
		}
		private ICommand newimageCommand;
		public ICommand NewImageCommand{
			get { 
				Debug.WriteLine ("newtextCommand");
				return newtextCommand ?? (newimageCommand = new Command(async () => await ExecuteNewImageCommand())); 
			}
		}

		public async Task ExecuteNewImageCommand(){
			Debug.WriteLine ("Started ExecuteNewImageCommand");
			SettingsImage.Source = image;
		}
	}
}

