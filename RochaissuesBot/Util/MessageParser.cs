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
		public static async Task<Message> Parse(Message message)
		{
			if (message.Text.ToLower().Trim() == "/help")
			{
				HelpCommand help = new HelpCommand();
				return help.Execute(message);
			}
			if (message.Text.ToLower().Trim() == "/clear")
			{
				ClearCommand clr = new ClearCommand();
				return clr.Execute(message);
			}

			if (message.GetBotUserData<bool>(SearchCommand.KEY))
				return await Conversation.SendAsync(message, MakeSearchRoot);

			if (message.GetBotUserData<bool>(ListCommand.KEY))
				return await Conversation.SendAsync(message, MakeListRoot);

			string messageText = message.Text.Trim();
			if (messageText.StartsWith(":") && messageText.EndsWith(":") && messageText.IndexOf(' ') == -1) //it's an emoji
				return message.CreateReplyMessage($"{messageText} to you too {message.From.Name}");

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
						message.SetBotConversationData(ListCommand.KEY, true);
						return await Conversation.SendAsync(message, MakeListRoot);
					case "search":
					case "find":
						message.SetBotConversationData(SearchCommand.KEY, true);
						return await Conversation.SendAsync(message, MakeSearchRoot);
					default:
						break;
				}

				if (intent != null)
					return intent.Execute(message);
			}
			
			//I need to use natural language recognition here with LUIS.ai
			LuisResponse luis = LuisManager.Parse(message);
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

			return intent.Execute(message);
		}

		internal static IFormDialog<SearchCommand> MakeSearchRoot()
		{
			return FormDialog.FromForm(SearchCommand.MakeForm);
		}

		internal static IFormDialog<ListCommand> MakeListRoot()
		{
			return FormDialog.FromForm(ListCommand.MakeForm);
		}
	}
}