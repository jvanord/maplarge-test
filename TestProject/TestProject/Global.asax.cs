using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Newtonsoft.Json.Serialization;

namespace TestProject
{
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			RouteConfig.RegisterRoutes(RouteTable.Routes);

			// Camel-Case Return Values
			GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ContractResolver
				= new CamelCasePropertyNamesContractResolver();
		}
	}
}
