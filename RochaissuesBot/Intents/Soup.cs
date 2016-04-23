using System.Collections.Generic;
using Microsoft.Bot.Connector;
using GXIssueTrackingBot.Util;

namespace GXIssueTrackingBot.Intents
{
	public class Soup : BaseIntent
	{
		public override Message Execute(Message message)
		{
			Message msg = message.CreateReplyMessage();
			msg.Text = $"**NO SOUP FOR YOU!** {message.From.Name}"; // Text property is Markdown!
			Attachment att = new Attachment();
			att.ContentType = "image/png";
			att.ContentUrl = BotConfiguration.SOUP_NAZI; // "http://fsartech/Goomez/images/soup_nazi.jpg";

			List<Attachment> attachments = new List<Attachment>();
			attachments.Add(att);

			msg.Attachments = attachments;

			return msg;
		}
	}
}