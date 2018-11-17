using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace TestProject.Models
{
	/// <summary>Detailed information on a given file.</summary>
	public class FileDetail
	{
		private FileInfo _fileInfo;
		public FileDetail(FileInfo fileInfo) => _fileInfo = fileInfo;

		public string FileName => _fileInfo.Name;

		/// <summary>Read-only. Gets the MIME content-type for the current file.</summary>
		public string ContentType => MimeMapping.GetMimeMapping(FileName);

		/// <summary>Reads the full contents of the current file.</summary>
		public async Task<byte[]> GetContents() => await Task.Run(() => File.ReadAllBytes(_fileInfo.FullName));
	}
}