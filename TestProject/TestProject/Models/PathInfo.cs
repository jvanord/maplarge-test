using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestProject.Models
{
	public class PathInfo
	{
		public string Path { get; set; }
		public List<string> Children { get; internal set; } = new List<string>();
		public List<string> Files { get; internal set; } = new List<string>();
	}
}