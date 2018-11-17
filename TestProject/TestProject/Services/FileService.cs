using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TestProject.Models;

namespace TestProject.Services
{
	/// <summary>Handles operation on files.</summary>
	public class FileService : ServiceBase
	{
		public FileService() : base() { }

		/// <summary></summary>
		public async Task<FileDetail> GetFile(string path)
		{
			if (!path.StartsWith("\\")) path = "\\" + path;
			var fileInfo = new FileInfo(Path.Combine(RootServerPath, path.Substring(1))); // Path.Combine doesn't like leading slashes in the second parameter
			return await Task.Run(() => {
				if (!fileInfo.Exists) throw new FileNotFoundException();
				return new FileDetail(fileInfo);
			});
			
		}

		/// <summary>Deletes a given path and all its contents.</summary>
		public async Task Delete(string path)
		{
			if (string.IsNullOrEmpty(path)) path = "\\";
			else if (!path.StartsWith("\\")) path = "\\" + path;
			var file = new FileInfo(Path.Combine(RootServerPath, path.Substring(1))); // Path.Combine doesn't like leading slashes in the second parameter
			await Task.Run(() =>
			{
				if (!file.Exists) throw new FileNotFoundException();
				file.Delete();
			});
		}
	}
}