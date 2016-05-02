using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using GXIssueTrackingBot.Util;
using Microsoft.Bot.Connector;

namespace GXIssueTrackingBot.LUIS
{
	public class LuisManager
	{
		public static LuisResponse Parse(Message message)
		{
			string url = $"https://api.projectoxford.ai/luis/v1/application?id={BotConfiguration.LUIS_KEY}&subscription-key={BotConfiguration.LUIS_SUBSCRIPTION}&q={message.Text}";
			WebClient wc = new WebClient();
			Stream response = wc.OpenRead(url);
			DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(LuisResponse));
			LuisResponse luis = ser.ReadObject(response) as LuisResponse;

			return luis;
		}
	}
}