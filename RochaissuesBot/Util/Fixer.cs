using System.Collections.Generic;

namespace GXIssueTrackingBot.Util
{
	public class Fixer
	{
		static Dictionary<string, string> replaceableChars = new Dictionary<string, string>()
		{
			{ ":","" },
			{"á","a" },
			{"é","e" },
			{"í","i" },
			{"ó","o" },
			{"ú","u" },
		};

		public static string Sanitize(string message)
		{
			foreach (var item in replaceableChars)
				message = message.Replace(item.Key, item.Value);

			return message;
		}
	}
}