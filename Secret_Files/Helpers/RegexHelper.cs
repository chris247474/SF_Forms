using System;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Secret_Files
{
	public static class RegexHelper
	{
		public const string COMPLETESINGLESPECIALCHARREGEX = "\\(*\\)*\\**\\'*\\`*\\~*\\.*\\,*\\?*\\:*\\;*\\\\*\\/*\\[*\\]*\\{*\\}*\\<*\\>*\\+*\\!*\\@*\\&*\\^*\\&*\\%*\\$*\\#*\\|*\\-*\\=*\\\"*\\ *";
		public const string SINGLEDIGIT = "\\d";

		public static string RemoveSpecialCharAndSingleDigits(string input){
			var SpecialRegex = new Regex (COMPLETESINGLESPECIALCHARREGEX);
			var DigitRegex = new Regex (SINGLEDIGIT);
			var result = SpecialRegex.Replace (DigitRegex.Replace (input, ""), "");
			Debug.WriteLine ("processed string {0}", result);
			return result;
		}

		public static bool MatchesOfficialSecretFilesThreadName(string input){
			return string.Equals (RegexHelper.RemoveSpecialCharAndSingleDigits (input.ToLower ()), Values.SECRETFILESOFFICIALTHREADNAMENOSPACES);
		}
	}
}

