using System;
using Microsoft.Bot.Connector;

namespace GXIssueTrackingBot.Intents.Command
{
	public class HelpCommand : BaseIntent
	{
		public override Message Execute(Message message)
		{
			ClearCommand clr = new ClearCommand();
			Message msg = clr.Execute(message);

			msg.Text = $"Hello {message.From.Name}! I'm Genexus Issue Tracking Bot.{Environment.NewLine}{Environment.NewLine}You can ask me for issues assigned to you in a natural language like _'show me issues open in salto assigned to <your issue tracking username>_'.{Environment.NewLine}{Environment.NewLine}Also, you can send me the title of an issue (subject in your email) and I'll try to find it and show you the ID and link to the issue.{Environment.NewLine}{Environment.NewLine}There's also a couple of commands you can try like **/search**  and **/list**. Those must start with the backslash and I'll guide you through the search.{Environment.NewLine}{Environment.NewLine}Type **/help** anytime to see this message again";

			return msg;
		}
	}
}