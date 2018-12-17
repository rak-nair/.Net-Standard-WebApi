using System;
using System.Web.Http.Routing;

namespace AssignmentAPI.Services
{
    //Class for building Paging Links for the GET operations.
    public class PagingLinkBuilder
    {
        #region Properties
        public Uri FirstPage { get; private set; }
        public Uri LastPage { get; private set; }
        public Uri NextPage { get; private set; }
        public Uri PreviousPage { get; private set; }
        #endregion

        #region Constructor
        public PagingLinkBuilder(UrlHelper urlHelper, string routeName, object routeValues, int pageNo, int pageSize, long totalRecordCount)
        {
            // Determine total number of pages
            var pageCount = totalRecordCount > 0
                ? (int)Math.Ceiling(totalRecordCount / (double)pageSize)
                : 0;

            // Create page links 
            FirstPage = new Uri(
                urlHelper.Link(routeName, new HttpRouteValueDictionary(routeValues)
                {
                    {"pageNo", 1},
                    {"pageSize", pageSize}
                }));
            LastPage = new Uri(
                urlHelper.Link(routeName, new HttpRouteValueDictionary(routeValues)
                {
                    {"pageNo", pageCount},
                    {"pageSize", pageSize}
                }));

            if (pageNo > 1)
            {
                PreviousPage = new Uri(urlHelper.Link(routeName, new HttpRouteValueDictionary(routeValues)
                {
                    {"pageNo", pageNo - 1},
                    {"pageSize", pageSize}
                }));
            }

            if (pageNo < pageCount)
            {
                NextPage = new Uri(
                    urlHelper.Link(routeName, new HttpRouteValueDictionary(routeValues)
                    {
                        {"pageNo", pageNo + 1},
                        {"pageSize", pageSize}
                    }));
            }
        }
        #endregion
    }
}