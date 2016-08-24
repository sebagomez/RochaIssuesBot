using System;
using System.Threading.Tasks;
using GXIssueTrackingBot.Intents;
using GXIssueTrackingBot.Intents.Command;
using GXIssueTrackingBot.LUIS;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;

namespace GXIssueTrackingBot.Util
{
	public class MessageParser
	{
		//https://channel9.msdn.com/Events/Build/2016/B821
		public static async Task<Activity> Parse(Activity activity)
		{
			if (activity.Text.ToLower().Trim() == "/help")
			{
				HelpCommand help = new HelpCommand();
				return help.Execute(activity);
			}
			if (activity.Text.ToLower().Trim() == "/clear")
			{
				ClearCommand clr = new ClearCommand();
				return clr.Execute(activity);
			}

			ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
			StateClient state = activity.GetStateClient();
			BotData data = state.BotState.GetUserData(activity.ChannelId, activity.From.Id);

			if (data.GetProperty<bool>(SearchCommand.KEY))
			{
				await Conversation.SendAsync(activity, MakeSearchRoot);
				return null;
			}
			if (data.GetProperty<bool>(ListCommand.KEY))
			{
				await Conversation.SendAsync(activity, MakeListRoot);
				return null;
			}

			string messageText = activity.Text.Trim();
			if (messageText.StartsWith(":") && messageText.EndsWith(":") && messageText.IndexOf(' ') == -1) //it's an emoji
			{
				await connector.Conversations.ReplyToActivityAsync(activity.CreateReply($"{messageText} to you too {activity.From.Name}"));
				return null;
			}

			BaseIntent intent = null;
			if (messageText.StartsWith("/")) // it's a command
			{
				string command = messageText.Substring(1).Trim().ToLower();
				switch (command)
				{
					case "help": //it should get here, but just in case
						intent = new HelpCommand();
						break;
					case "clear":
						intent = new ClearCommand();
						break;
					case "list":
					case "show":
						data.SetProperty<bool>(ListCommand.KEY, true);
						state.BotState.SetUserData(activity.ChannelId, activity.From.Id, data);
						await Conversation.SendAsync(activity, MakeListRoot);
						break;
					case "search":
					case "find":
						data.SetProperty<bool>(SearchCommand.KEY, true);
						state.BotState.SetUserData(activity.ChannelId, activity.From.Id, data);
						await Conversation.SendAsync(activity, MakeSearchRoot);
						break;
					default:
						break;
				}

				if (intent != null)
					return intent.Execute(activity);
				else
					return null;
			}

			//I need to use natural language recognition here with LUIS.ai
			LuisResponse luis = LuisManager.Parse(activity);
			Intent luisIntent = luis.intents[0];

			switch (luisIntent.intent)
			{
				case "Soup":
					intent = new Soup();
					break;
				case "ShowIssues":
					intent = new ShowIssues(luis);
					break;
				case "SearchByText":
					intent = new SearchByText();
					break;
				default:
					intent = new None();
					break;
			}

			return intent.Execute(activity);
		}

		internal static IDialog<SearchCommand> MakeSearchRoot()
		{
			return Chain.From(() => FormDialog.FromForm(SearchCommand.MakeForm));
		}

		internal static IDialog<ListCommand> MakeListRoot()
		{
			return Chain.From(() => FormDialog.FromForm(ListCommand.MakeForm));
		}
	}
}