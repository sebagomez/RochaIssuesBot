using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GXIssueTrackingBot.IssueTracking.SDTs
{

	public class SearchResult
	{
		public Gxsearchresults GXSearchResults { get; set; }
	}

	public class Gxsearchresults
	{
		public string DocumentsCount { get; set; }
		public string TotalPages { get; set; }
		public Document[] Documents { get; set; }
		public string SearchTime { get; set; }
	}

	public class Document
	{
		public string Description { get; set; }
		public string Id { get; set; }
		public string Source { get; set; }
		public Property1[] Properties { get; set; }
	}

	public class Property1
	{
		public string Key { get; set; }
		public string Value { get; set; }
		public string Type { get; set; }
		public string Boost { get; set; }
		public bool Searchable { get; set; }
		public bool Redundant { get; set; }
	}

}