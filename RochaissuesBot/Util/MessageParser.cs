using GXIssueTrackingBot.Intents;
using GXIssueTrackingBot.LUIS;
using Microsoft.Bot.Connector;

namespace GXIssueTrackingBot.Util
{
	public class MessageParser
	{
		//https://channel9.msdn.com/Events/Build/2016/B821
		public static Message Parse(Message message)
		{
			if (message.Text.StartsWith(":") && message.Text.EndsWith(":") && message.Text.IndexOf(' ') == -1) //it's an emoji
				return message.CreateReplyMessage($"{message.Text} to you too {message.From.Name}");

			BaseIntent intent;
			//I need to use natural language recognition here with LUIS.ai
			//NuGet Micrsoft.Bot.Builder must be used for form completition, like where the pizza must go to, or how large, or toppins
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
	}
}