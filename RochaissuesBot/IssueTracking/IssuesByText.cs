using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using GXIssueTrackingBot.IssueTracking.SDTs;
using GXIssueTrackingBot.Util;

namespace GXIssueTrackingBot.IssueTracking
{
	public class IssuesByText
	{
		public static string Search(string text)
		{
			string messageText = Fixer.Sanitize(text);
			string url = $"{BotConfiguration.ISSUE_TRACKING}/rest/issuesbytext";

			WebClient wc = new WebClient();
			wc.Headers.Add(HttpRequestHeader.ContentType, "application/json");
			wc.Credentials = new NetworkCredential(BotConfiguration.ITUSERNAME, BotConfiguration.ITPASSWORD);

			string response = null;

			try
			{
				response = wc.UploadString(url, "POST", "{\"SearchWords\":\"" + messageText + "\"}");
			}
			catch (WebException wex)
			{
				return $"Ooops! Genexus issue tracking returned an error. _{wex.Message}_"; ;
			}
			catch (Exception ex)
			{
				return $"Ooops! _{ex.Message}_"; ;
			}

			DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(SearchResult));
			SearchResult result = null;
			byte[] bytes = Encoding.UTF8.GetBytes(response);
			using (var stream = new MemoryStream(bytes))
				result = ser.ReadObject(stream) as SearchResult;

			int count = int.Parse(result.GXSearchResults.DocumentsCount);
			if (count == 0)
				return $"I'm sorry, I couldn't find anything with that title";

			if (count > 100)
				return $"I've found waaay too many issues ({count}), please refine your search";

			StringBuilder builder = new StringBuilder();
			foreach (var doc in result.GXSearchResults.Documents)
			{
				int id = int.Parse(doc.Id.Substring(doc.Id.LastIndexOf('?') + 1));
				builder.Append($"[{id}]({doc.Id}) {doc.Description}{Environment.NewLine}{Environment.NewLine}");
			}

			return builder.ToString();
		}
	}
}