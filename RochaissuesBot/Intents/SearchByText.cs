using GXIssueTrackingBot.IssueTracking;
using Microsoft.Bot.Connector;

namespace GXIssueTrackingBot.Intents
{
	public class SearchByText : BaseIntent
	{
		public override Activity Execute(Activity activity)
		{
			return activity.CreateReply(IssuesByText.Search(activity.Text));
		}
	}
}