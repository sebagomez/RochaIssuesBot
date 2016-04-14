using Microsoft.Bot.Connector;
using RochaissuesBot.Actions;

namespace RochaissuesBot.Util
{
	public class MessageParser
	{
		public enum Action
		{
			Find

		}

		public static Message Parse(Message msg)
		{
			//the "magic" of parsing is done here
			IssueAction act;
			if (msg.Text.Trim().ToLower() == "soup")
				act = new Soup();
			else
				act = new IssuesByText();

			return act.Execute(msg);
		}
	}
}