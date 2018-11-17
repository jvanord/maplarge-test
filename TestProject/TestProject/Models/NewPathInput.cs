using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestProject.Models
{
	/// <summary>Valid input parameters to create a new directory.</summary>
	public class NewPathInput
	{
		public string Path { get; set; }
		public string Name { get; set; }
	}
}