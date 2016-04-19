using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace RochaissuesBot.Util
{
	public class BotConfiguration
	{
		public static string LUIS_KEY { get; } = ConfigurationManager.AppSettings["LuisKey"];
		public static string LUIS_SUBSCRIPTION { get; } = ConfigurationManager.AppSettings["LuisSubscription"];
	}
}