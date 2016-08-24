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

		public override Activity Execute(Activity activity)
		{
			Activity msg = GetReplyMessage(activity);
			msg.Text = IssuesDP.Query(User, Project, Status, Category, Type);

			return msg;
		}

		Activity GetReplyMessage(Activity activity)
		{
			StateClient state = activity.GetStateClient();
			BotData data = state.BotState.GetUserData(activity.ChannelId, activity.From.Id);

			Activity msg = activity.CreateReply();
			string user = data.GetProperty<string>(USER_CODE);
			string project = data.GetProperty<string>(PROJECT_CODE);
			string status = data.GetProperty<string>(STATUS);
			string type = data.GetProperty<string>(TYPE);
			string category = data.GetProperty<string>(CATEGORY);

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

			data.SetProperty<string>(USER_CODE, User ?? "");
			data.SetProperty<string>(PROJECT_CODE, Project ?? "");
			data.SetProperty<string>(STATUS, Status ?? "");
			data.SetProperty<string>(TYPE, Type ?? "");
			data.SetProperty<string>(CATEGORY, Category ?? "");

			state.BotState.SetUserData(activity.ChannelId, activity.From.Id, data);

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