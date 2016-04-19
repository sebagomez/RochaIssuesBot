using Microsoft.Bot.Connector;
using RochaissuesBot.IssueTracking;
using RochaissuesBot.LUIS;

namespace RochaissuesBot.Util
{
	public class MessageParser
	{
		//https://channel9.msdn.com/Events/Build/2016/B821
		public static Message Parse(Message msg)
		{
			IssueAction act;
			//I need to use natural language recognition here with LUIS.ai
			//NuGet Micrsoft.Bot.Builder must be used for form completition, like where the pizza must go to, or how large, or toppins
			LuisResponse luis = LuisManager.Parse(msg);
			Intent intent = luis.intents[0];

			switch (intent.intent)
			{
				case "Soup":
					act = new Soup();
					break;
				case "ShowIssues":
					act = new QueryIssueTracking(luis);
					break;
				case "SearchByText":
					act = new IssuesByText();
					break;
				default:
					act = new Unknown();
					break;
			}

			return act.Execute(msg);
		}
	}
}