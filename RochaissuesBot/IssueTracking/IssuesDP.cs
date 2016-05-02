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

			if (!string.IsNullOrEmpty(category))
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

			if (sdt.Error && !string.IsNullOrEmpty(sdt.Message))
				builder.Append(sdt.Message);

			if (sdt.Issues.Count > 100)
				builder.Append($"I've found waaay too many issues ({sdt.Issues.Count}), please refine your search");

			if (sdt.Issues.Count > 0 && sdt.Issues.Count <= 100)
			{
				builder.Append($"I've found {sdt.Issues.Count} ");

				if (!string.IsNullOrEmpty(status))
					builder.Append($"{status} ");

				builder.Append("issues ");

				if (!string.IsNullOrEmpty(type))
					builder.Append($"({type})");

				if (!string.IsNullOrEmpty(status))
					builder.Append($" {status.ToLower()} ");
				if (!string.IsNullOrEmpty(project))
					builder.Append($"in {project} ");
				if (!string.IsNullOrEmpty(user))
					builder.Append($"assigned to {user} ");
				if (!string.IsNullOrEmpty(category))
					builder.Append($"with category '{category}'");

				builder.Append($"{Environment.NewLine}{Environment.NewLine}");

				foreach (Issue issue in sdt.Issues)
				{
					builder.Append($"[{issue.Issueid}]({BotConfiguration.ISSUE_TRACKING}/viewissue.aspx?{issue.Issueid}) {issue.Issuetitle}{Environment.NewLine}{Environment.NewLine}");
				}
			}

			return builder.ToString();
		}
	}
}