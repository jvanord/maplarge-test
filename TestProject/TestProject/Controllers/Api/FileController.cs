using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TestProject.Models;
using TestProject.Services;

namespace TestProject.Controllers.Api
{
	public class FileController : ApiController
	{

		[HttpPost, Route("api/file")]
		public async Task<FileData> Post(string path = null)
		{
			if(HttpContext.Current.Request.Files == null || HttpContext.Current.Request.Files.Count < 1)
				throw new HttpResponseException(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.InternalServerError,
					ReasonPhrase = "No File Uploaded"
				});
			if (path == null) path = HttpContext.Current.Request["path"];
			return await new FileService().SaveFile(path, HttpContext.Current.Request.Files[0]);
		}

		[HttpDelete, Route("api/file")]
		public async Task Delete(string path = null)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(path))
					throw new HttpResponseException(new HttpResponseMessage
					{
						StatusCode = HttpStatusCode.InternalServerError,
						ReasonPhrase = "No File to Delete Specified"
					});
				await new FileService().Delete(path);
			}
			catch (FileNotFoundException)
			{
				throw new HttpResponseException(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.NotFound,
					ReasonPhrase = "Specified File Not Found"
				});
			}
			catch (Exception ex)
			{
				throw new HttpResponseException(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.InternalServerError,
					ReasonPhrase = ex.Message
				});
			}
		}
	}
}