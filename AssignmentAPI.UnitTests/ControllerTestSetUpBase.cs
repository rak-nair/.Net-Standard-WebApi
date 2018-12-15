using AssignmentAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;

namespace AssignmentAPI.UnitTests
{
    public class ControllerTestSetUpBase
    {
        protected static BaseApiController SetUpDummyHttpConfiguration(BaseApiController sut)
        {
            sut.Request = new HttpRequestMessage();
            sut.Request.Properties["MS_HttpConfiguration"] = new HttpConfiguration();

            return sut;
        }

        protected static BaseApiController SetUpDummyPaging(BaseApiController sut, string routeName)
        {
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://eye4talent.com/api/");
            var route = config.Routes.MapHttpRoute(routeName, "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary
            {
                {"id", Guid.Empty},
                {"controller", "organization"}
            });
            sut.ControllerContext = new HttpControllerContext(config, routeData, request);
            UrlHelper urlHelper = new UrlHelper(request);
            sut.Request = request;
            sut.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
            sut.Request.Properties[HttpPropertyKeys.HttpRouteDataKey] = routeData;
            sut.Url = new UrlHelper(request);

            return sut;
        }
    }
}
