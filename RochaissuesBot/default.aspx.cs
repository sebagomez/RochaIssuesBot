using System;
using System.Reflection;
using System.Web.UI;

namespace GXIssueTrackingBot
{
	public partial class _default : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				Assembly assembly = Assembly.GetExecutingAssembly();
				AssemblyName name = assembly.GetName();
				Version ver = name.Version;

				Page.Title = $"Genexus Issue Tracking bot v{ver.Major}.{ver.Minor}.{ver.Build}.{ver.Revision}";
				lblVersion.Text = ver.ToString();
			}
		}
	}
}