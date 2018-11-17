using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using TestProject.Models;

namespace TestProject.Services
{
	public class DirectoryService
	{
		private static string _rootServerPath;

		public DirectoryService()
		{
			if (string.IsNullOrEmpty(_rootServerPath))
			{
				var browseRoot = ConfigurationManager.AppSettings.Get("BrowseRoot");
				if (string.IsNullOrWhiteSpace(browseRoot)) throw new Exception("BrowseRoot Not Configured");
				_rootServerPath = browseRoot.StartsWith("~") ? HostingEnvironment.MapPath(browseRoot) : browseRoot;
			}
		}

		public async Task<PathInfo> GetPath(string path)
		{
			if (string.IsNullOrEmpty(path)) path = "\\";
			else if (!path.StartsWith("\\")) path = "\\" + path;
			var directory = new DirectoryInfo(Path.Combine(_rootServerPath, path.Substring(1))); // Path.Combine doesn't like leading slashes in the second parameter
			return await Task.Run(() =>
			{
				if (!directory.Exists) throw new DirectoryNotFoundException();
				return new PathInfo
				{
					Path = path,
					Children = directory.GetDirectories().Select(info => Path.Combine(path, info.Name)).ToList(),
					Files = directory.GetFiles().Select(info => info.Name + info.Extension).ToList()
				};
			});
		}

		public async Task<PathInfo> GetRootPath() => await GetPath("\\");
	}
}