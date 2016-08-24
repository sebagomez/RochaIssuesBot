using Microsoft.Bot.Connector;

namespace GXIssueTrackingBot.Intents.Command
{
	public class ClearCommand : BaseIntent
	{
		public override Activity Execute(Activity activity)
		{
			Activity msg = activity.CreateReply("Ok now, you can start from scratch");
			StateClient state = activity.GetStateClient();
			BotData data = state.BotState.GetUserData(activity.ChannelId, activity.From.Id);
			data.SetProperty<string>(PROJECT_CODE, null);
			data.SetProperty<string>(USER_CODE, null);
			data.SetProperty<string>(STATUS, null);
			data.SetProperty<string>(TYPE, null);
			data.SetProperty<string>(CATEGORY, null);

			data.SetProperty<bool>(SearchCommand.KEY, false);
			data.SetProperty<bool>(ListCommand.KEY, false);

			state.BotState.SetUserData(activity.ChannelId, activity.From.Id, data);

			return msg;
		}
	}
}