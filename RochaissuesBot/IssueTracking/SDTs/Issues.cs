using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GXIssueTrackingBot.IssueTracking.SDTs
{
	public class Issue
	{
		public string Issueid { get; set; }
		public string Issuetitle { get; set; }
		public int Issuetype { get; set; }
		public string IssuetypeDesc { get; set; }
		public int Issuepriority { get; set; }
		public string IssuepriorityDesc { get; set; }
		public string Issueassignedto { get; set; }
		public string Projectid { get; set; }
		public string Issuetext { get; set; }
		public int Issuestatus { get; set; }
		public string IssueStatusDesc { get; set; }
		public string Issuepromiseddate { get; set; }
		public string Issuereporteddate { get; set; }
		public string Issuereportedby { get; set; }
		public string Issuefixeddate { get; set; }
		public string Issuefixedby { get; set; }
		public string Issuetesteddate { get; set; }
		public string Issuetestedby { get; set; }
		public string Issueupdateddate { get; set; }
		public string Issueupdatedby { get; set; }
		public string Issuecloseddate { get; set; }
		public string Issueclosedby { get; set; }
	}

	public class IssuesSDT
	{
		public List<Issue> Issues { get; set; }
		public bool Error { get; set; }
		public string Message { get; set; }
	}
}