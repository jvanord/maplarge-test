using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace TestProject.Services
{
	public class ServiceBase
	{
		protected static string RootServerPath { get; private set; }

		protected ServiceBase()
		{
			if (string.IsNullOrEmpty(RootServerPath))
			{
				var browseRoot = ConfigurationManager.AppSettings.Get("BrowseRoot");
				if (string.IsNullOrWhiteSpace(browseRoot)) throw new Exception("BrowseRoot Not Configured");
				RootServerPath = browseRoot.StartsWith("~") ? HostingEnvironment.MapPath(browseRoot) : browseRoot;
			}
		}
	}
}