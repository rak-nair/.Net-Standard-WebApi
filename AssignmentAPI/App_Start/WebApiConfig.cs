using System.Web.Http;

namespace AssignmentAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "Matches",
                routeTemplate: "api/matches/{matchid}",
                defaults: new { controller = "matches", matchid = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "Players",
                routeTemplate: "api/players/{playerid}",
                defaults: new { controller = "players", playerid = RouteParameter.Optional }
            );
            
        }
    }
}
