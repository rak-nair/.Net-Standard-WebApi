using AssignmentAPI.Models;
using AssignmentAPI.Services;
using System.Web.Http;

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
    }
}
