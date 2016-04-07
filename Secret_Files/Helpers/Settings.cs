// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Xamarin.Forms;

namespace Secret_Files.Helpers
{
  /// <summary>
  /// This is the Settings static class that can be used in your Core solution or in any
  /// of your client applications. All settings are laid out the same exact way with getters
  /// and setters. 
  /// </summary>
  public static class Settings
  {
    private static ISettings AppSettings
    {
      get
      {
        return CrossSettings.Current;
      }
    }

	public static bool IsLoggedIn { 
		get { 
			return !string.IsNullOrWhiteSpace (UserId);
		}
	}

    #region Setting Constants

    private const string SettingsKey = "settings_key";
    private static readonly string SettingsDefault = string.Empty;
	
	const string UserIdKey = "userid";
	static readonly string UserIdDefault = string.Empty;

	const string AuthTokenKey = "authtoken";
	static readonly string AuthTokenDefault = string.Empty;

	const string username = "Username";
	static readonly string usernameDefault = string.Empty;

	const string profilepicKey = "postsample.png";
	static readonly string profilepicDefault = "postsample.png";

    #endregion
	
	public static string ProfilePic
	{
		get { return AppSettings.GetValueOrDefault<string>(profilepicKey, profilepicDefault); }
		set  { AppSettings.AddOrUpdateValue<string>(profilepicKey, value); }
	}

	public static string AuthToken
	{
		get { return AppSettings.GetValueOrDefault<string>(AuthTokenKey, AuthTokenDefault); }
		set  { AppSettings.AddOrUpdateValue<string>(AuthTokenKey, value); }
	}

	public static string UserId
	{
		get { return AppSettings.GetValueOrDefault<string>(UserIdKey, UserIdDefault);  }
		set { AppSettings.AddOrUpdateValue<string>(UserIdKey, value); }
	}

	public static string Username
	{
		get { return AppSettings.GetValueOrDefault<string>(username, usernameDefault);  }
		set { AppSettings.AddOrUpdateValue<string>(username, value); }
	}

	


    public static string GeneralSettings
    {
      get
      {
        return AppSettings.GetValueOrDefault<string>(SettingsKey, SettingsDefault);
      }
      set
      {
        AppSettings.AddOrUpdateValue<string>(SettingsKey, value);
      }
    }

  }
}