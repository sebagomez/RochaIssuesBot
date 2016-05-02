using System;
using GXIssueTrackingBot.IssueTracking;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;

namespace GXIssueTrackingBot.Intents.Command
{
	[Serializable]
	public class SearchCommand
	{
		public const string KEY = "search";

		[Prompt("Enter the title of the issue you are looking for")]
		[Describe("Issue Title")]
		public string Text { get; set; }

		public static IForm<SearchCommand> MakeForm()
		{
			OnCompletionAsyncDelegate<SearchCommand> goSearch = async (context, search) =>
			{
				context.ConversationData.SetValue<bool>(KEY, false);
				await context.PostAsync(IssuesByText.Search(search.Text));
			};

			FormBuilder<SearchCommand> form = new FormBuilder<SearchCommand>();

			return form
				.Message("I'll be looking for an issue title in full text search...")
				.AddRemainingFields()
				.Message("Thanks, I'll go ask Genexus issues now...")
				.OnCompletionAsync(goSearch)
				.Build();
		}


	}
}