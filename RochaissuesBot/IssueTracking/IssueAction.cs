using Microsoft.Bot.Connector;

namespace RochaissuesBot.IssueTracking
{
	public abstract class IssueAction
	{
		public virtual Message Execute(Message message)
		{
			return message.CreateReplyMessage($"Sorry {message.From.Name} :(, don't know what '{message.Text}' means (yet)");
		}
	}
}