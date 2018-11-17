using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestProject.Models
{
	/// <summary>Valid input parameters to upload new files.</summary>
	public class UploadInput
	{
		public string Path { get; set; }
		public IEnumerable<HttpPostedFile> Files { get; set; }
	}
}