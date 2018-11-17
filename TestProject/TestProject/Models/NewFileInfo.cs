using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestProject.Models
{
	/// <summary>Information on newly created files.</summary>
	public class NewFileInfo
	{
		public string Path { get; set; }
		public string Name { get; set; }
		public bool Success { get; internal set; }
	}
}