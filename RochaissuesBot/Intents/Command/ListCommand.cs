using System;
using GXIssueTrackingBot.IssueTracking;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;

namespace GXIssueTrackingBot.Intents.Command
{
	[Serializable]
	public class ListCommand
	{
		public const string KEY = "list";

		[Prompt("What's the user you want to see issues assigned to? _(set 'none' if you don't want to filter by user)_")]
		public string User { get; set; }
		[Prompt("What's the project? (e.g: salto, xev3u8) _(set 'none' if you don't want to filter by project)_")]
		public string Project { get; set; }
		[Prompt("What's the status? (e.g: open, fixed, tested) _(set 'none' if you don't want to filter by status)_")]
		public string Status { get; set; }
		[Prompt("Any specific category you want to filter? (e.g: broken, gxlook) _(set 'none' if you don't want to filter by catgeory)_")]
		public string Category { get; set; }
		[Prompt("Any specific type you want to filter? (e.g: bug, feature) _(set 'none' if you don't want to filter by type)_")]
		public string Type { get; set; }


		public static IForm<ListCommand> MakeForm()
		{
			OnCompletionAsyncDelegate<ListCommand> goSearch = async (context, list) =>
			{
				context.UserData.SetValue<bool>(KEY, false);
				await context.PostAsync(IssuesDP.Query(list.User == "none" ? "" : list.User, list.Project == "none" ? "" : list.Project, list.Status == "none" ? "" : list.Project, list.Category == "none" ? "" : list.Category, list.Type == "none" ? "" : list.Type));
				
			};

			FormBuilder<ListCommand> form = new FormBuilder<ListCommand>();

			return form
				.Message("I'll ask Genexus Issue Tracking system for issues matching your criteria")
				.AddRemainingFields()
				.Message("Thanks, I'll go ask Genexus issues now...")
				.OnCompletionAsync(goSearch)
				.Build();
		}


	}
}