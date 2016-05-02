using GXIssueTrackingBot.IssueTracking;
using Microsoft.Bot.Connector;

namespace GXIssueTrackingBot.Intents
{
	public class SearchByText : BaseIntent
	{
		public override Message Execute(Message message)
		{
			return message.CreateReplyMessage(IssuesByText.Search(message.Text));
		}
	}
}