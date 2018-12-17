using AssignmentAPI.Models;
using AssignmentAPI.Services;
using System.Web.Http;
using System.Web.Http.Routing;

namespace AssignmentAPI.Controllers
{
    //Base for common operations
    public abstract class BaseApiController : ApiController
    {
        #region Properties
        protected ModelFactory TheModelFactory { get; }

        protected IAssignmentData TheRepository { get; }

        protected ErrorResponses TheErrorResponses { get; }
        #endregion

        #region Constructor
        public BaseApiController(IAssignmentData data)
        {
            TheRepository = data;
            TheErrorResponses = new ErrorResponses();
            TheModelFactory = new ModelFactory(data, TheErrorResponses);
            TheErrorResponses = new ErrorResponses();
        }
        #endregion

        #region Methods
        protected static PagingLinkBuilder CreatePageLinks (UrlHelper urlHelper, string routeName, object routeValues, int pageNo, int pageSize, long totalRecordCount)
        {
            return new PagingLinkBuilder(urlHelper, routeName, routeValues, pageNo, pageSize, totalRecordCount);
        }
        #endregion
    }
}
