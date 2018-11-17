using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TestProject.Models;

namespace TestProject.Services
{
	public class FileService : ServiceBase
	{
		public FileService() : base() { }

		public async Task<FileDetail> GetFile(string path)
		{
			if (!path.StartsWith("\\")) path = "\\" + path;
			var fileInfo = new FileInfo(Path.Combine(RootServerPath, path.Substring(1))); // Path.Combine doesn't like leading slashes in the second parameter
			return await Task.Run(() => {
				if (!fileInfo.Exists) throw new FileNotFoundException();
				return new FileDetail(fileInfo);
			});
			
		}
	}
}