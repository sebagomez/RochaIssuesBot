using GXIssueTrackingBot.IssueTracking;
using GXIssueTrackingBot.LUIS;
using Microsoft.Bot.Connector;

namespace GXIssueTrackingBot.Intents
{
	public class ShowIssues : BaseIntent
	{
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
			msg.Text = IssuesDP.Query(User, Project, Status, Category, Type);

			return msg;
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