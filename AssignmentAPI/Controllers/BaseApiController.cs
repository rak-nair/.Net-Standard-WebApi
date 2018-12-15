using AssignmentAPI.Models;
using AssignmentAPI.Services;
using System.Web.Http;
using System.Web.Http.Routing;

namespace AssignmentAPI.Controllers
{
    public abstract class BaseApiController : ApiController
    {
        public BaseApiController(IAssignmentData data)
        {
            TheRepository = data;
            TheModelFactory = new ModelFactory(data);
        }

        protected ModelFactory TheModelFactory { get; } 

        protected IAssignmentData TheRepository { get; }

        protected static PagingLinkBuilder CreatePageLinks (UrlHelper urlHelper, string routeName, object routeValues, int pageNo, int pageSize, long totalRecordCount)
        {
            return new PagingLinkBuilder(urlHelper, routeName, routeValues, pageNo, pageSize, totalRecordCount);
        }
    }
}
