using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using GXIssueTrackingBot.Intents;
using GXIssueTrackingBot.IssueTracking.SDTs;
using GXIssueTrackingBot.Util;

namespace GXIssueTrackingBot.IssueTracking
{
	public class IssuesDP
	{
		public static string Query(string user, string project, string status, string category, string type)
		{
			string url = $"{BotConfiguration.ISSUE_TRACKING}/rest/issuesdp?";
			if (!string.IsNullOrEmpty(user))
				url += $"{BaseIntent.USER_CODE}={user}&";

			if (!string.IsNullOrEmpty(project))
				url += $"{BaseIntent.PROJECT_CODE}={project}&";

			if (!string.IsNullOrEmpty(status))
				url += $"{BaseIntent.STATUS}={status}&";

			if (!string.IsNullOrEmpty(type))
				url += $"{BaseIntent.TYPE}={type}&";

			//if (!string.IsNullOrEmpty(category)) //right now (26/05/2016) there's an issue with null parameters in rest services
				url += $"{BaseIntent.CATEGORY}={category}&";

			url = url.Substring(0, url.Length - 1); //remove the last '&'

			WebClient wc = new WebClient();
			wc.Credentials = new NetworkCredential(BotConfiguration.ITUSERNAME, BotConfiguration.ITPASSWORD);
			Stream response = null;
			try
			{
				response = wc.OpenRead(url);
			}
			catch (WebException wex)
			{
				return $"Ooops! Genexus issue tracking returned an error. _{wex.Message}_"; ;
			}
			catch (Exception ex)
			{
				return $"Ooops! _{ex.Message}_"; ;
			}

			DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(IssuesSDT));
			IssuesSDT sdt = ser.ReadObject(response) as IssuesSDT;

			StringBuilder builder = new StringBuilder();
			string queryString = SearchedText(status, type, project, user, category, sdt.Issues.Count);

			if (sdt.Error)
			{
				builder.Append($"I'm sorry. I could not find any {queryString}");

				if (!string.IsNullOrEmpty(sdt.Message))
					builder.Append($"The server said _{sdt.Message}_");
			}

			if (sdt.Issues.Count > 100)
				builder.Append($"I've found waaay too many issues ({sdt.Issues.Count}), please refine your search");

			if (sdt.Issues.Count > 0 && sdt.Issues.Count <= 100)
			{
				builder.Append($"I've found {sdt.Issues.Count} {queryString}");

				builder.Append($"{Environment.NewLine}{Environment.NewLine}");

				foreach (Issue issue in sdt.Issues)
				{
					builder.Append($"[{issue.Issueid}]({BotConfiguration.ISSUE_TRACKING}/viewissue.aspx?{issue.Issueid}) {issue.Issuetitle}{Environment.NewLine}{Environment.NewLine}");
				}
			}

			return builder.ToString();
		}

		static string SearchedText(string status, string type, string project, string user, string category, int count)
		{
			StringBuilder builder = new StringBuilder();
			if (!string.IsNullOrEmpty(status))
				builder.Append($"{status} ");

			builder.Append(count == 1 ? "issue " : "issues ");
			//builder.Append("issues ");

			if (!string.IsNullOrEmpty(type))
				builder.Append($"({type})");

			if (!string.IsNullOrEmpty(project))
				builder.Append($"in {project} ");
			if (!string.IsNullOrEmpty(user))
				builder.Append($"assigned to {user} ");
			if (!string.IsNullOrEmpty(category))
				builder.Append($"with category '{category}'");

			return builder.ToString();
		}
	}
}