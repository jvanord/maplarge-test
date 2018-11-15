using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestProject.Controllers
{
	public class DefaultController : Controller
	{
		// SPA Page
		public ActionResult Index() => View();
	}
}