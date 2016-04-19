using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Connector;

namespace RochaissuesBot.IssueTracking
{
	public class Unknown : IssueAction
	{
		public override Message Execute(Message msg)
		{
			return base.Execute(msg);
		}
	}
}