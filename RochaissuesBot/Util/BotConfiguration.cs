using System.Configuration;

namespace GXIssueTrackingBot.Util
{
	public class BotConfiguration
	{
		public static string LUIS_KEY { get; } = ConfigurationManager.AppSettings["LuisKey"];
		public static string LUIS_SUBSCRIPTION { get; } = ConfigurationManager.AppSettings["LuisSubscription"];
		public static string ISSUE_TRACKING { get; } = ConfigurationManager.AppSettings["IssueTrackingLocation"];
		public static string SOUP_NAZI { get; } = ConfigurationManager.AppSettings["SoupNaziLocation"];
		public static string ITUSERNAME { get; } = ConfigurationManager.AppSettings["IssueTrackingUserName"];
		public static string ITPASSWORD { get; } = ConfigurationManager.AppSettings["IssueTrackingPassword"];
	}
}