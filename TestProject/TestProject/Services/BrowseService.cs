using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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

		public PathInfo GetPath(string path)
		{
			var dirPath = path.StartsWith("\\")
				? Path.Combine(_rootServerPath, path.Substring(1)) // Path.Combine doesn't like leading slashes in the second parameter
				: Path.Combine(_rootServerPath, path);
			var directory = new DirectoryInfo(dirPath);
			if (!directory.Exists) throw new DirectoryNotFoundException();
			return new PathInfo
			{
				Path = path,
				Children = directory.GetDirectories().Select(info => "\\" + Path.Combine(path, info.Name)).ToList(),
				Files = directory.GetFiles().Select(info => info.Name + info.Extension).ToList()
			};
		}

		public PathInfo GetRootPath() => GetPath("\\");
	}
}