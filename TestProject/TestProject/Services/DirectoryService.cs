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
	/// <summary>Manages Operations on Folders/Directories</summary>
	public class DirectoryService : ServiceBase
	{
		public DirectoryService() : base() { }

		/// <summary>Gets information and contents of a given path.</summary>
		public async Task<PathInfo> GetPath(string path)
		{
			if (string.IsNullOrEmpty(path)) path = "\\";
			else if (!path.StartsWith("\\")) path = "\\" + path;
			var directory = new DirectoryInfo(Path.Combine(RootServerPath, path.Substring(1))); // Path.Combine doesn't like leading slashes in the second parameter
			return await Task.Run(() =>
			{
				if (!directory.Exists) throw new DirectoryNotFoundException();
				return new PathInfo
				{
					Path = path,
					Children = directory.GetDirectories().Select(info => Path.Combine(path, info.Name)).ToList(),
					Files = directory.GetFiles().Select(info => new FileData
					{
						Name = info.Name,
						Path = path,
						Size = (int)info.Length
					}).ToList()
				};
			});
		}

		/// <summary>Gets information and contents of the application's root browse path.</summary>
		public async Task<PathInfo> GetRootPath() => await GetPath("\\");

		/// <summary>Creates a child directory for a give path.</summary>
		public async Task<PathInfo> CreateChild(string path, string name)
		{
			if (string.IsNullOrEmpty(name)) throw new Exception("New Folder Name not Specified");
			if (string.IsNullOrEmpty(path)) path = "\\";
			else if (!path.StartsWith("\\")) path = "\\" + path;
			var parent = new DirectoryInfo(Path.Combine(RootServerPath, path.Substring(1)));
			return await Task.Run(() =>
			{
				if (!parent.Exists) throw new DirectoryNotFoundException();
				var newDir = Directory.CreateDirectory(Path.Combine(RootServerPath, path.Substring(1), name));
				return new PathInfo
				{
					Path = path + name,
					Children = new List<string>(),
					Files = new List<FileData>()
				};
			});
		}

		/// <summary>Deletes a given path and all its contents.</summary>
		public async Task Delete(string path)
		{
			if (string.IsNullOrEmpty(path)) path = "\\";
			else if (!path.StartsWith("\\")) path = "\\" + path;
			var directory = new DirectoryInfo(Path.Combine(RootServerPath, path.Substring(1))); // Path.Combine doesn't like leading slashes in the second parameter
			await Task.Run(() =>
			{
				if (!directory.Exists) throw new DirectoryNotFoundException();
				directory.Delete(true);
			});
		}
	}
}