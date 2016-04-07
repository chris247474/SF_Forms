using System;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Acr.UserDialogs;
using System.Threading.Tasks;
using System.Diagnostics;
using Xamarin.Forms;

#if __ANDROID__
using Android.Graphics;
#endif

namespace Secret_Files
{
	public class CameraService
	{
		
		public CameraService ()
		{
			Init ();
		}
		async void Init(){
			await CrossMedia.Current.Initialize ();
		}
		public async Task<string> ChooseImage(ContentPage page){
			try{
				var result = await page.DisplayActionSheet ("Choose", "OK", "Cancel", new string[]{"Take Photo", "From Gallery"});

				if(string.Equals (result, "Take Photo")){
					string file = await TakePic ();
					Debug.WriteLine(file);
					if(!string.IsNullOrWhiteSpace (file)){
						//img.Source = file;//assign to group image
						return file;
						//save image to server
					}else{
						UserDialogs.Instance.WarnToast("Couldn't load image. Pls try again");
					}
				}else if(string.Equals (result, "From Gallery")){
					string file = await ChoosePic ();
					Debug.WriteLine(file);
					if(!string.IsNullOrWhiteSpace (file)){
						//img.Source = file;//assign to group image
						return file;
						//save image to server
					}else{
						UserDialogs.Instance.WarnToast("Couldn't load image. Pls try again");
					}
				}
			}catch(Exception e){
				Debug.WriteLine ("Choose Image exception "+e.Message);
			}
			return null;
		}
		public async Task<string> TakePic(){
			if(CrossMedia.Current.IsCameraAvailable){
				try{
					var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
						{
							Directory = "Secret Files",
							Name = "GroupProfilePic"+DateTime.Now+".jpg",
							SaveToAlbum = true
						});
					if (file == null) {
						UserDialogs.Instance.WarnToast ("Something went wrong, I can't find your camera");
						return null;
					}
					return file.Path;
				}catch(Exception e){
					//Debug.WriteLine ("Camera exited");
					return null;
				}
			}else{
				UserDialogs.Instance.WarnToast ("Something went wrong, I can't find your camera");
				return null;
			}
		}
		public async Task<string> ChoosePic(){
			if(CrossMedia.Current.IsPickPhotoSupported){
				try{
					var file = await CrossMedia.Current.PickPhotoAsync();
					if (file == null){
						UserDialogs.Instance.WarnToast ("Something went wrong, I can't find your gallery");
						return null;
					}
					return file.Path;
				}catch(Exception e){
					//Debug.WriteLine ("Pick Photo exited");
					return null;
				}
			}else{
				UserDialogs.Instance.WarnToast ("Something went wrong. I can't detect your gallery");
				return null;
			}
		}

	}
}

