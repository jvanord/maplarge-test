using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TestProject.Services;

namespace TestProject.Controllers.Api
{
	public class FileController : ApiController
	{
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
			catch (FileNotFoundException ex)
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