using Microsoft.Bot.Connector;

namespace GXIssueTrackingBot.Intents.Command
{
	public class ClearCommand : BaseIntent
	{
		public override Message Execute(Message message)
		{
			Message msg = message.CreateReplyMessage("Ok now, you can start from scratch");
			msg.SetBotUserData(PROJECT_CODE, null);
			msg.SetBotUserData(USER_CODE, null);
			msg.SetBotUserData(STATUS, null);
			msg.SetBotUserData(TYPE, null);
			msg.SetBotUserData(CATEGORY, null);

			msg.SetBotUserData(SearchCommand.KEY, false);
			msg.SetBotUserData(ListCommand.KEY, false);

			return msg;
		}
	}
}