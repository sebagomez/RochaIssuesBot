using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using Microsoft.Bot.Connector;
using RochaissuesBot.IssueTracking.SDTs;
using RochaissuesBot.Util;

namespace RochaissuesBot.IssueTracking
{
	public class IssuesByText : IssueAction
	{
		public override Message Execute(Message message)
		{
			string url = $"{BotConfiguration.ISSUE_TRACKING}/rest/issuesbytext";
			WebClient wc = new WebClient();
			wc.Headers.Add(HttpRequestHeader.ContentType, "application/json");
			wc.Credentials = new NetworkCredential(BotConfiguration.ITUSERNAME, BotConfiguration.ITPASSWORD);
			string response = wc.UploadString(url,"POST", "{\"SearchWords\":\"" + message.Text + "\"}");

			DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(SearchResult));
			SearchResult result = null;
			byte[] bytes = Encoding.UTF8.GetBytes(response);
			using (var stream = new MemoryStream(bytes))
				result = ser.ReadObject(stream) as SearchResult;

			Message msg = message.CreateReplyMessage();
			int count = int.Parse(result.GXSearchResults.DocumentsCount);
			if (count == 0)
			{
				msg.Text = "I'm sorry, I couldn't find anything with that description";
				return msg;
			}


			if (count > 100)
			{
				msg.Text = $"I've found waaay too many issues ({count}), please redifine your search";
				return msg;
			}

			if (count > 0)
			{
				foreach (var doc in result.GXSearchResults.Documents)
				{
					int id = int.Parse(doc.Id.Substring(doc.Id.LastIndexOf('?') + 1));
					msg.Text += $"[{id}]({doc.Id}) {doc.Description}{Environment.NewLine}{Environment.NewLine}";
				}

				return msg;
			}

			return base.Execute(message);
		}
	}
}