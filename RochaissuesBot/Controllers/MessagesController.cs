using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using GXIssueTrackingBot.Intents.Command;
using GXIssueTrackingBot.Util;
using Microsoft.Bot.Connector;

namespace GXIssueTrackingBot
{
	[BotAuthentication]
	public class MessagesController : ApiController
	{
		/// <summary>
		/// POST: api/Messages
		/// Receive a message from a user and reply to it
		/// </summary>
		public async Task<Message> Post([FromBody]Message message)
		{
			if (message.Type == "Message")
			{
				// calculate something for us to return
				int length = (message.Text ?? string.Empty).Length;

				// return our reply to the user
				string[] greetings = { "hello", "hi" };
				if (greetings.Contains(message.Text.ToLower().Trim()))
				{
					string name = message.From.ChannelId == "facebook" ? "facebook user" : message.From.Name;
					return message.CreateReplyMessage($"Not many people says hi to me, {message.Text.ToLower()} to you too {name}!");
				}
				return await MessageParser.Parse(message);
			}
			else
			{
				return HandleSystemMessage(message);
			}
		}

		private Message HandleSystemMessage(Message message)
		{
			Message reply = null;
			if (message.Type == "Ping")
			{
				reply = message.CreateReplyMessage();
				reply.Type = "Ping";
			}
			else if (message.Type == "DeleteUserData")
			{
				// Implement user deletion here
				// If we handle user deletion, return a real message
			}
			else if (message.Type == "BotAddedToConversation")
			{
				HelpCommand cmd = new HelpCommand();
				return cmd.Execute(message);
				
			}
			else if (message.Type == "BotRemovedFromConversation")
			{
			}
			else if (message.Type == "UserAddedToConversation")
			{
			}
			else if (message.Type == "UserRemovedFromConversation")
			{
			}
			else if (message.Type == "EndOfConversation")
			{
			}

			return reply;
		}
	}
}