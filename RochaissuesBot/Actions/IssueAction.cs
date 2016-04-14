using Microsoft.Bot.Connector;

namespace RochaissuesBot.Actions
{
	public abstract class IssueAction
	{
		public virtual Message Execute(Message msg)
		{
			return new Message() { Text = $"Sorry {msg.From.Name} :(, don't know what '{msg.Text}' means (yet)" };
		}
	}
}