using System.Collections.Generic;
using Microsoft.Bot.Connector;
using GXIssueTrackingBot.Util;

namespace GXIssueTrackingBot.Intents
{
	public class Soup : BaseIntent
	{
		public override Activity Execute(Activity activity)
		{
			Activity msg = activity.CreateReply();
			msg.Text = $"**NO SOUP FOR YOU!** {activity.From.Name}"; // Text property is Markdown!
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