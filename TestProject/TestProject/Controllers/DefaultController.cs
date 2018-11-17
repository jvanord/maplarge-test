using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TestProject.Services;

namespace TestProject.Controllers
{
	public class DefaultController : Controller
	{
		// SPA Page
		public ActionResult Index() => View();

		// File Download

		public async Task<ActionResult> Download(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
				return Content("No File Specified");
			try
			{
				var file = await new FileService().GetFile(path);
				return File(await file.GetContents(), file.ContentType);
			}
			catch (FileNotFoundException)
			{
				return Content("File Not Found");
			}
		}
	}
}