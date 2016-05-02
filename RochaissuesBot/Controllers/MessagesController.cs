using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Utilities;
using Newtonsoft.Json;
using GXIssueTrackingBot.Util;

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
				if (message.Text.ToLower().Trim() == "hello")
					return message.CreateReplyMessage($"Hello to you too {message.From.Name}! What's up!");

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
				reply = message.CreateReplyMessage();
				reply.Text = $"Welcome {message.From.Name}";
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