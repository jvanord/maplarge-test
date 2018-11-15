using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TestProject.Models;
using TestProject.Services;

namespace TestProject.Controllers.Api
{
	public class BrowseController : ApiController
	{
		[Route("api/browse")]
		public PathInfo Get(string path = null)
		{
			try
			{
				return string.IsNullOrWhiteSpace(path)
					? new BrowseService().GetRootPath()
					: new BrowseService().GetPath(path);
			}
			catch(DirectoryNotFoundException)
			{
				throw new HttpResponseException(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.NotFound,
					ReasonPhrase = "Directory Does Not Exist"
				});
			}
		}
	}
}