using System;

using Xamarin.Forms;
using System.Collections.Generic;
using System.Diagnostics;
using Secret_Files.Helpers;
using Microsoft.WindowsAzure.MobileServices;

namespace Secret_Files
{
	public class MasterStartPage : MasterDetailPage
	{
		string privacytext, whatis;
		public MasterStartPage ()
		{
			this.Title = "Secret Files";
			this.Detail = new GroupsPage ();
			this.Master = new ContentPage {
				BackgroundColor = Color.FromHex (Values.BACKGROUNDLIGHTSILVER),
				Title = "Secret Files",
				Icon = "ic_menu_white_24dp.png",
				Content = createView ()
			};
		}

		ScrollView createView(){
			privacytext = "How does Secret Files ensure that the information or posts you provide stay secret?\n\n" +
			"Privacy is a common and major concern in social media nowadays. We at Secret Files are committed to providing a secure platform " +
			"that allows anyone to speak their mind no matter where or who you are. " +
			"Secret Files aims to keep you and your data as anonymous as possible by;\n\n\n" +
			"\t1.\tNot storing your data in the first place. \n\nOther social networks and organizations make it their mission to protect their users data " +
			"stored on their servers. We, however, avoid that hurdle completely by not even asking for your email address. All you need to use Secret Files " +
			"is... Absolutely nothing! That's it. Simple right? No email, no phone number, nothing. We can't even read what you post unless we're in the same" +
			" Secret Files thread (just like any user). \n\n\t2.\tEncrypting your data! \n\n" +
			"While not even we know who you are, we also don't have access to anything you post! We employ state of the art encryption algorithms to ensure " +
			"that no one at Secret Files can even read your content unless that person is already a part of the same Secret Files thread it was posted in" +
			".\n\n\n\n\nContingency plans: \n\n\n\tIn the event that someone's taken A LOT (seriously, A LOT)" +
			" of time and effort to find out who you are (pretty determined if you ask us), " +
			"we got you covered by;\n\n\t1.\tProviding the anonymous user with self destruct tools. \nIf at anytime you realize that your identity might be discovered " +
			"from something you post, you have the ability to self destruct any content you may have posted anywhere on Secret Files." +
			" You may also self destruct your entire profile at anytime.\n\n2.\tIn the event you feel that #2 isn't enough to protect your anonymity, " +
			"then after self destructing your posts, you may simply make another account by going to the Settings and tapping 'Reincarnate Me'.\n" +
			"\n\t3.\tIf for some reason, you have reason to believe someone is stalking you for something you posted or said, we have a decoy feature!! (More on this)\n\n\n" +
			"FAQ:\n\nQ: Can't someone track my IP address or that meta deta stuff I read about online?\n\nA: Secret Files doesn't store your IP address or even any " +
			"\"extra\" data. When you download the Secret Files mobile app, it records nothing from you or your phone except your posts (which are encrypted, " +
			"but even if someone does happen to hack our servers, they still won't know who you are because not even we know!). ";

			whatis = "";

			return new ScrollView {
				Content = new StackLayout{
				Orientation = StackOrientation.Vertical,
				HorizontalOptions = LayoutOptions.Fill,
				VerticalOptions = LayoutOptions.StartAndExpand,
				BackgroundColor = Color.White,
					Children = { 
						UIBuilder.CreateSetting ("ic_vpn_key_grey_500_24dp.png", "Protecting Your Privacy", 
							new TapGestureRecognizer{Command = new Command(() =>
								Navigation.PushAsync (new InfoPage(privacytext))
							)}),
						UIBuilder.CreateSeparator (Color.Gray, 0.5),
						UIBuilder.CreateSetting ("ic_person_pin_grey_500_24dp.png", "Change Profile Pic", 
							new TapGestureRecognizer{Command = new Command(() =>
								Navigation.PushAsync (new ProfilePicChooser())
							)}),
						UIBuilder.CreateSetting ("ic_person_grey_500_24dp.png", "Change Username", 
							new TapGestureRecognizer{Command = new Command(() =>
								Util.SaveNewUserName ()
							)}),
						UIBuilder.CreateSetting ("", "Change Colors", 
							new TapGestureRecognizer{Command = new Command(() =>
								Util.SaveNewUserName ()
							)}),
						UIBuilder.CreateSetting ("ic_sentiment_very_satisfied_orange_500_24dp.png", "Contact Team Secret Files!", 
							new TapGestureRecognizer{Command = new Command(() =>
								{}
							)}),
						/*UIBuilder.CreateSetting ("ic_phonelink_erase_grey_500_24dp.png", "Wipe me",  
							new TapGestureRecognizer{Command = new Command(() =>
								
							)}),*/
						//listView
					}
				}
			};
		}
	}
}


