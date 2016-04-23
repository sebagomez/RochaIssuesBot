using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GXIssueTrackingBot.Util
{
	public class Fixer
	{
		public static string Sanitize(string message)
		{
			return message.Replace(":", "");
		}
	}
}