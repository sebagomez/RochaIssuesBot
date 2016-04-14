using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using Microsoft.Bot.Connector;
using RochaissuesBot.SDTs;

namespace RochaissuesBot.Actions
{
	public class IssuesByText : IssueAction
	{
		public override Message Execute(Message message)
		{
			//http://localhost/GeneXusIssueTrackingTilo.NetEnvironment/rest/issuesdp?IssueTitle=Error%20de%20compilacion

			string url = $"http://localhost/GeneXusIssueTrackingTilo.NetEnvironment/rest/issuesdp?IssueTitle={message.Text}";
			WebClient wc = new WebClient();
			Stream response = wc.OpenRead(url);
			DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(IssuesSDT));
			IssuesSDT sdt = ser.ReadObject(response) as IssuesSDT;

			Message msg = new Message();
			if (sdt.Error && !string.IsNullOrEmpty(sdt.Message))
			{
				msg.Text = sdt.Message;
				return msg;
			}


			if (sdt.Issues.Count > 50)
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