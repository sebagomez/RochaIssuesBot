using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using Microsoft.Bot.Connector;
using GXIssueTrackingBot.IssueTracking.SDTs;
using GXIssueTrackingBot.LUIS;
using GXIssueTrackingBot.Util;

namespace GXIssueTrackingBot.Intents
{
	public class ShowIssues : BaseIntent
	{
		public string User { get; set; }
		public string Project { get; set; }
		public string Status{ get; set; }
		public string Type { get; set; }

		public ShowIssues(LuisResponse luis)
		{
			foreach (var entity in luis.entities)
			{
				switch (entity.type)
				{
					case "project":
						Project = entity.entity;
						break;
					case "user":
						User = entity.entity;
						break;
					case "status":
						Status = entity.entity;
						break;
					default:
						break;
				}
			}
		}

		public override Message Execute(Message message)
		{
			string url = $"{BotConfiguration.ISSUE_TRACKING}/rest/issuesdp?";
			if (!string.IsNullOrEmpty(User))
				url += "UserCode=" + User + "&";

			if (!string.IsNullOrEmpty(Project))
				url += "ProjectCode=" + Project + "&";

			if (!string.IsNullOrEmpty(Status))
				url += "Status=" + Status + "&";

			if (!string.IsNullOrEmpty(Type))
				url += "Type=" + Type + "&";

			url = url.Substring(0, url.Length - 1); //remove the last '&'

			WebClient wc = new WebClient();
			wc.Credentials = new NetworkCredential(BotConfiguration.ITUSERNAME, BotConfiguration.ITPASSWORD);
			Stream response = wc.OpenRead(url);
			DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(IssuesSDT));
			IssuesSDT sdt = ser.ReadObject(response) as IssuesSDT;

			Message msg = message.CreateReplyMessage();
			if (sdt.Error && !string.IsNullOrEmpty(sdt.Message))
			{
				msg.Text = sdt.Message;
				return msg;
			}


			if (sdt.Issues.Count > 100)
			{
				msg.Text = $"I've found waaay too many issues ({sdt.Issues.Count}), please redifine your search";
				return msg;
			}

			if (sdt.Issues.Count > 0)
			{
				foreach (Issue issue in sdt.Issues)
				{
					msg.Text += $"[{issue.Issueid}]({BotConfiguration.ISSUE_TRACKING}/viewissue.aspx?{issue.Issueid}) {issue.Issuetitle}{Environment.NewLine}{Environment.NewLine}";
				}

				return msg;
			}

			return base.Execute(message);
		}
	}
}