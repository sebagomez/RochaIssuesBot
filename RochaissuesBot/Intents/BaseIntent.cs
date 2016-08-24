using Microsoft.Bot.Connector;

namespace GXIssueTrackingBot.Intents
{
	public abstract class BaseIntent
	{
		public const string USER_CODE = "UserCode";
		public const string PROJECT_CODE = "ProjectCode";
		public const string STATUS = "Status";
		public const string TYPE = "Type";
		public const string CATEGORY = "Category";

		public virtual Activity Execute(Activity activity)
		{
			return activity.CreateReply($"Sorry {activity.From.Name} :(, don't know what '{activity.Text}' means (yet)");
		}
	}
}