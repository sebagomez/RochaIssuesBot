using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using Microsoft.Bot.Connector;
using GXIssueTrackingBot.IssueTracking.SDTs;
using GXIssueTrackingBot.LUIS;
using GXIssueTrackingBot.Util;
using System.Text;

namespace GXIssueTrackingBot.Intents
{
	public class ShowIssues : BaseIntent
	{
		const string USER_CODE = "UserCode";
		const string PROJECT_CODE = "ProjectCode";
		const string STATUS = "Status";
		const string TYPE = "Type";
		const string CATEGORY = "Category";

		public string User { get; set; }
		public string Project { get; set; }
		public string Status { get; set; }
		public string Type { get; set; }
		public string Category { get; set; }

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
					case "type":
						Type = entity.entity;
						break;
					case "category":
						Category = entity.entity;
						break;
					default:
						break;
				}
			}
		}

		public override Message Execute(Message message)
		{
			Message msg = GetReplyMessage(message);
			string url = $"{BotConfiguration.ISSUE_TRACKING}/rest/issuesdp?";
			if (!string.IsNullOrEmpty(User))
				url += $"{USER_CODE}={User}&";

			if (!string.IsNullOrEmpty(Project))
				url += $"{PROJECT_CODE}={Project}&";

			if (!string.IsNullOrEmpty(Status))
				url += $"{STATUS}={Status}&";

			if (!string.IsNullOrEmpty(Type))
				url += $"{TYPE}={Type}&";

			if (!string.IsNullOrEmpty(Category))
				url += $"{CATEGORY}={Category}&";

			url = url.Substring(0, url.Length - 1); //remove the last '&'

			WebClient wc = new WebClient();
			wc.Credentials = new NetworkCredential(BotConfiguration.ITUSERNAME, BotConfiguration.ITPASSWORD);
			Stream response = wc.OpenRead(url);
			DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(IssuesSDT));
			IssuesSDT sdt = ser.ReadObject(response) as IssuesSDT;

			if (sdt.Error && !string.IsNullOrEmpty(sdt.Message))
			{
				msg.Text = sdt.Message;
				return msg;
			}

			if (sdt.Issues.Count > 100)
			{
				msg.Text = $"I've found waaay too many issues ({sdt.Issues.Count}), please refine your search";
				return msg;
			}

			if (sdt.Issues.Count > 0)
			{
				StringBuilder builder = new StringBuilder($"I've found {sdt.Issues.Count} ");

				if (!string.IsNullOrEmpty(Status))
					builder.Append($"{Status} ");

				builder.Append("issues ");

				if (!string.IsNullOrEmpty(Type))
					builder.Append($"({Type})");

				if (!string.IsNullOrEmpty(Status))
					builder.Append($" {Status.ToLower()} ");
				if (!string.IsNullOrEmpty(Project))
					builder.Append($"in {Project} ");
				if (!string.IsNullOrEmpty(User))
					builder.Append($"assigned to {User} ");
				if (!string.IsNullOrEmpty(Category))
					builder.Append($"with category '{Category}'");

				msg.Text = builder.ToString();
				msg.Text += $"{Environment.NewLine}{Environment.NewLine}";

				foreach (Issue issue in sdt.Issues)
				{
					msg.Text += $"[{issue.Issueid}]({BotConfiguration.ISSUE_TRACKING}/viewissue.aspx?{issue.Issueid}) {issue.Issuetitle}{Environment.NewLine}{Environment.NewLine}";
				}

				return msg;
			}

			return base.Execute(message);
		}

		Message GetReplyMessage(Message message)
		{
			Message msg = message.CreateReplyMessage();
			string user = message.GetBotUserData<string>(USER_CODE);
			string project = message.GetBotUserData<string>(PROJECT_CODE);
			string status = message.GetBotUserData<string>(STATUS);
			string type = message.GetBotUserData<string>(TYPE);
			string category = message.GetBotUserData<string>(CATEGORY);

			if (IsSingleParameter)
			{
				if (string.IsNullOrEmpty(User) && !string.IsNullOrEmpty(user))
					User = user;
				if (string.IsNullOrEmpty(Project) && !string.IsNullOrEmpty(project))
					Project = project;
				if (string.IsNullOrEmpty(Status) && !string.IsNullOrEmpty(status))
					Status = status;
				if (string.IsNullOrEmpty(Type) && !string.IsNullOrEmpty(type))
					Type = type;
				if (string.IsNullOrEmpty(Category) && !string.IsNullOrEmpty(category))
					Category = category;
			}

			msg.SetBotUserData(USER_CODE, User);
			msg.SetBotUserData(PROJECT_CODE, Project);
			msg.SetBotUserData(STATUS, Status);
			msg.SetBotUserData(TYPE, Type);
			msg.SetBotUserData(CATEGORY, Category);

			return msg;
		}

		bool IsSingleParameter
		{
			get
			{
				return string.IsNullOrEmpty(User) && string.IsNullOrEmpty(Project) && string.IsNullOrEmpty(Status) && string.IsNullOrEmpty(Type) && !string.IsNullOrEmpty(Category) ||
					string.IsNullOrEmpty(User) && string.IsNullOrEmpty(Project) && string.IsNullOrEmpty(Status) && !string.IsNullOrEmpty(Type) && string.IsNullOrEmpty(Category) ||
					string.IsNullOrEmpty(User) && string.IsNullOrEmpty(Project) && !string.IsNullOrEmpty(Status) && string.IsNullOrEmpty(Type) && string.IsNullOrEmpty(Category) ||
					string.IsNullOrEmpty(User) && !string.IsNullOrEmpty(Project) && string.IsNullOrEmpty(Status) && string.IsNullOrEmpty(Type) && string.IsNullOrEmpty(Category) ||
					!string.IsNullOrEmpty(User) && string.IsNullOrEmpty(Project) && string.IsNullOrEmpty(Status) && string.IsNullOrEmpty(Type) && string.IsNullOrEmpty(Category);
			}
		}
	}
}