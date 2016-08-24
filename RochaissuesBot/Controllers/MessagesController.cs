using System;
using System.Linq;
using System.Net;
using System.Net.Http;
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
		public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
		{
			ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
			if (activity.GetActivityType() == ActivityTypes.Message)
			{
				// calculate something for us to return
				int length = (activity.Text ?? string.Empty).Length;

				// return our reply to the user
				string[] greetings = { "hello", "hi" };
				if (greetings.Contains(activity.Text.ToLower().Trim()))
				{
					string name = activity.From.Id == "facebook" ? "facebook user" : activity.From.Name;
					await connector.Conversations.ReplyToActivityAsync(activity.CreateReply($"Not many people says hi to me, {activity.Text.ToLower()} to you too {name}!"));
				}
				else
				{
					Activity reply = await MessageParser.Parse(activity);
					if (reply != null)
						await connector.Conversations.ReplyToActivityAsync(reply);
				}
			}
			else
			{
				Activity reply = HandleSystemMessage(activity);
				if (reply != null)
					await connector.Conversations.ReplyToActivityAsync(reply);
			}
			return Request.CreateResponse(HttpStatusCode.OK);

		}

		private Activity HandleSystemMessage(Activity activity)
		{
			Activity reply = null;
			switch (activity.GetActivityType())
			{
				case ActivityTypes.Ping:
					reply = activity.CreateReply();
					reply.Type = ActivityTypes.Ping;
					reply.Text = "Haga Pum!";
					return reply;
				case ActivityTypes.Typing:
					reply = activity.CreateReply();
					reply.Type = ActivityTypes.Typing;
					reply.Text = "Answer me already...";
					return reply;
				case ActivityTypes.ContactRelationUpdate:
				case ActivityTypes.ConversationUpdate:
				case ActivityTypes.DeleteUserData:
				default:
					break;
			}

			return reply;
		}
	}
}