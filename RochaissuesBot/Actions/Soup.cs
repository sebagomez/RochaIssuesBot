using System.Collections.Generic;
using Microsoft.Bot.Connector;

namespace RochaissuesBot.Actions
{
	public class Soup : IssueAction
	{
		public override Message Execute(Message message)
		{
			Message msg = new Message();
			msg.Text = $"**NO SOUP FOR YOU!** {message.From.Name}"; // Text property is Markdown!
			Attachment att = new Attachment();
			att.ContentType = "image/png";
			att.ContentUrl = "http://fsartech/Goomez/images/soup_nazi.jpg";

			List<Attachment> attachments = new List<Attachment>();
			attachments.Add(att);

			msg.Attachments = attachments;

			return msg;
		}
	}
}