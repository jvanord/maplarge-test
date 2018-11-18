using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestProject.Models
{
	/// <summary>Information on files.</summary>
	public class FileData
	{
		public string Path { get; set; }
		public string Name { get; set; }
		public int Size { get; set; }
	}
}