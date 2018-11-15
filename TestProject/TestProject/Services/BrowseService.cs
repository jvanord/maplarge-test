using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using TestProject.Models;

namespace TestProject.Services
{
	public class BrowseService
	{
		private static string _rootServerPath;

		public BrowseService()
		{
			if (string.IsNullOrEmpty(_rootServerPath))
			{
				var browseRoot = ConfigurationManager.AppSettings.Get("BrowseRoot");
				if (string.IsNullOrWhiteSpace(browseRoot)) throw new Exception("BrowseRoot Not Configured");
				_rootServerPath = browseRoot.StartsWith("~") ? HostingEnvironment.MapPath(browseRoot) : browseRoot;
			}
		}

		public PathInfo GetPath(string path) => new PathInfo { Path = path, Childred = GetChildren(path) };

		public PathInfo GetRootPath() => GetPath(string.Empty);

		private List<string> GetChildren(string path) => new List<string>();
	}
}