using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using Microsoft.Bot.Connector;
using RochaissuesBot.IssueTracking.SDTs;
using RochaissuesBot.LUIS;

namespace RochaissuesBot.IssueTracking
{
	public class QueryIssueTracking : IssueAction
	{
		public string User { get; set; }
		public string Project { get; set; }
		public string Status{ get; set; }
		public string Type { get; set; }

		public QueryIssueTracking(LuisResponse luis)
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
			string url = $"http://localhost/GeneXusIssueTrackingTilo.NetEnvironment/rest/issuesdp?";
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
					msg.Text += $"[{issue.Issueid}](https://issues.genexus.com/viewissue.aspx?{issue.Issueid}) {issue.Issuetitle}{Environment.NewLine}{Environment.NewLine}";
				}

				return msg;
			}

			return base.Execute(message);
		}
	}
}