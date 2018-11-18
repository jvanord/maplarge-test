using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestProject.Models
{
	/// <summary>Information on the path and contents of a directory.</summary>
	public class PathInfo
	{
		public string Path { get; set; }
		public List<string> Children { get; internal set; } = new List<string>();
		public List<FileData> Files { get; internal set; } = new List<FileData>();
	}
}