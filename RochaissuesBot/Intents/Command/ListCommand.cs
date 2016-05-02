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

		[Optional]
		[Prompt("What's the user you want to see issues assigned to? {||}")]
		public string User { get; set; }
		[Optional]
		public string Project { get; set; }
		[Optional]
		public string Status { get; set; }
		[Optional]
		public string Category { get; set; }
		[Optional]
		public string Type { get; set; }


		public static IForm<ListCommand> MakeForm()
		{
			OnCompletionAsyncDelegate<ListCommand> goSearch = async (context, list) =>
			{
				context.ConversationData.SetValue<bool>(KEY, false);
				await context.PostAsync(IssuesDP.Query(list.User, list.Project, list.Status, list.Category, list.Type));
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