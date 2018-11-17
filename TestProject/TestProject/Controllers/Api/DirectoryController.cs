using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TestProject.Models;
using TestProject.Services;

namespace TestProject.Controllers.Api
{
	public class DirectoryController : ApiController
	{
		[HttpGet, Route("api/dir")]
		public async Task<PathInfo> Get(string path = null)
		{
			try
			{
				return string.IsNullOrWhiteSpace(path)
					? await new DirectoryService().GetRootPath()
					: await new DirectoryService().GetPath(path);
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

		[HttpPost, Route("api/dir")]
		public async Task<PathInfo> Post([FromBody]NewPathInput input)
		{
			try
			{
				return await new DirectoryService().CreateChild(input.Path, input.Name);
			}
			catch(Exception ex)
			{
				throw new HttpResponseException(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.InternalServerError,
					ReasonPhrase = ex.Message
				});

			}
		}

		[HttpDelete, Route("api/dir")]
		public async Task Delete(string path = null)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(path))
					throw new HttpResponseException(new HttpResponseMessage
					{
						StatusCode = HttpStatusCode.InternalServerError,
						ReasonPhrase = "No Path to Delete Specified"
					});
				await new DirectoryService().Delete(path);
			}

			catch (DirectoryNotFoundException ex)
			{
				throw new HttpResponseException(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.NotFound,
					ReasonPhrase = "Specified Directory Not Found"
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