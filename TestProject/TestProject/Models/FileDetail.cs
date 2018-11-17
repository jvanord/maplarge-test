using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace TestProject.Models
{
	public class FileDetail
	{
		private FileInfo _fileInfo;
		public FileDetail(FileInfo fileInfo) => _fileInfo = fileInfo;

		public string FileName => _fileInfo.Name;
		public string ContentType => MimeMapping.GetMimeMapping(FileName);

		public async Task<byte[]> GetContents() => await Task.Run(() => File.ReadAllBytes(_fileInfo.FullName));

	}
}