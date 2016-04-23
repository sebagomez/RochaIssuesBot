using Microsoft.Bot.Connector;

namespace GXIssueTrackingBot.Intents
{
	public abstract class BaseIntent
	{
		public virtual Message Execute(Message message)
		{
			return message.CreateReplyMessage($"Sorry {message.From.Name} :(, don't know what '{message.Text}' means (yet)");
		}
	}
}