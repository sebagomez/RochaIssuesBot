using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using Microsoft.Bot.Connector;
using RochaissuesBot.IssueTracking.SDTs;

namespace RochaissuesBot.IssueTracking
{
	public class IssuesByText : IssueAction
	{
		public override Message Execute(Message message)
		{
			string url = $"http://localhost/GeneXusIssueTrackingTilo.NetEnvironment/rest/issuesbytext";
			WebClient wc = new WebClient();
			wc.Headers.Add(HttpRequestHeader.ContentType, "application/json");

			string response = wc.UploadString(url, "{\"SearchWords\":\""+ message.Text +"\"}");

			//DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(IssuesSDT));

			Message msg = message.CreateReplyMessage();
			//if (sdt.Error && !string.IsNullOrEmpty(sdt.Message))
			//{
			//	msg.Text = sdt.Message;
			//	return msg;
			//}


			//if (sdt.Issues.Count > 50)
			//{
			//	msg.Text = $"I've found waaay too many issues ({sdt.Issues.Count}), please redifine your search";
			//	return msg;
			//}

			//if (sdt.Issues.Count > 0)
			//{
			//	foreach (Issue issue in sdt.Issues)
			//	{
			//		msg.Text += $"[{issue.Issueid}](https://issues.genexus.com/viewissue.aspx?{issue.Issueid}) {issue.Issuetitle}{Environment.NewLine}{Environment.NewLine}";
			//	}

			//	return msg;
			//}

			return base.Execute(message);
		}
	}
}