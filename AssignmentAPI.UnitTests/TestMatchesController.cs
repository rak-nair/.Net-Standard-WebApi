using AssignmentAPI.Controllers;
using AssignmentAPI.Data.Entities;
using AssignmentAPI.Models;
using AssignmentAPI.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Web.Http.Results;
using System.Web.Http.Routing;

namespace AssignmentAPI.UnitTests
{
    [TestClass]
    public class TestMatchesController
    {
        [TestMethod]
        public void GetAllMatches_ShouldReturnAllMatches()
        {
            var sut = new MatchesController(new InMemoryAssignmentData());
           
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://eye4talent.com/api/");
            var route = config.Routes.MapHttpRoute("Matches", "api/{controller}/{id}");
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

            var result = sut.GetAllMatches()
                as OkNegotiatedContentResult<PagedMatchViewModel>;

            Assert.IsNotNull(result.Content);
            Assert.AreEqual(2, result.Content.Matches.Count);
        }

        [TestMethod]
        public async Task GetMatch_ValidID_Success()
        {
            var sut = new MatchesController(new InMemoryAssignmentData());

            var result = await sut.GetMatch(1)
                as OkNegotiatedContentResult<MatchEntity>;

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Content.MatchID);
        }

        [TestMethod]
        public async Task GetMatch_InvalidID_BadRequest()
        {
            var sut = new MatchesController(new InMemoryAssignmentData());

            var result = await sut.GetMatch(99);

            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
        }

        [TestMethod]
        public async Task AddMatch_ValidData_Success()
        {
            var sut = new MatchesController(new InMemoryAssignmentData());
            MatchModel demoMatch = ReturnValidDemoMatch();

            var result = await sut.AddMatch(demoMatch)
                as CreatedAtRouteNegotiatedContentResult<MatchEntity>;

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.RouteValues["matchid"]);
        }

        [TestMethod]
        public void AddMatch_InvalidData_InvalidModelState()
        {
            var sut = new MatchesController(new InMemoryAssignmentData());
            var demoMatch = ReturnInvalidDemoMatch();
            sut.Request = new HttpRequestMessage();
            sut.Request.Properties["MS_HttpConfiguration"] = new HttpConfiguration();

            sut.Validate(demoMatch);

            Assert.IsFalse(sut.ModelState.IsValid);
            Assert.AreEqual(2, sut.ModelState.Keys.Count);
        }

        [TestMethod]
        public async Task AddMatch_Duplicate_BadRequest()
        {
            var data = new InMemoryAssignmentData();
            var sut = new MatchesController(data);
            MatchModel demoMatch = new MatchModel
            {
                MatchDateTime = data.SampleMatch.MatchDateTime.ToString(),
                MatchTitle = data.SampleMatch.MatchTitle
            };

            var result = await sut.AddMatch(demoMatch);

            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
        }

        [TestMethod]
        public void GetPlayersInMatch_ValidID_ShouldReturnAllPlayers()
        {
            var sut = new MatchesController(new InMemoryAssignmentData());

            var result = sut.GetPlayersInMatch(1) as OkNegotiatedContentResult<List<MatchPlayerModel>>;

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Content.Count);
            Assert.AreEqual(1, result.Content[0].Player.PlayerID);
        }

        [TestMethod]
        public void GetPlayersInMatch_InvalidID_BadRequest()
        {
            var sut = new MatchesController(new InMemoryAssignmentData());

            var result = sut.GetPlayersInMatch(99);

            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
        }

        [TestMethod]
        public async Task AddPlayerToMatch_ValidData_Success()
        {
            var sut = new MatchesController(new InMemoryAssignmentData());

            var result = await sut.AddPlayerToMatch(1, 3) as CreatedAtRouteNegotiatedContentResult<MatchPlayerEntity>;

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.RouteValues["matchid"]);
            Assert.AreEqual(5, result.RouteValues["matchplayerid"]);
            Assert.AreEqual(3, result.Content.Player.PlayerID);

        }

        [TestMethod]
        public async Task AddPlayerToMatch_InvalidData_PlayerID_BadRequest()
        {
            var sut = new MatchesController(new InMemoryAssignmentData());

            var result = await sut.AddPlayerToMatch(1, 9);

            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
        }

        [TestMethod]
        public async Task AddPlayerToMatch_InvalidData_MatchID_BadRequest()
        {
            var sut = new MatchesController(new InMemoryAssignmentData());

            var result = await sut.AddPlayerToMatch(9, 9);

            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
        }

        [TestMethod]
        public async Task AddPlayerToMatch_Duplicate_BadRequest()
        {
            var sut = new MatchesController(new InMemoryAssignmentData());

            var result = await sut.AddPlayerToMatch(1, 1);

            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
        }

        MatchModel ReturnValidDemoMatch()
        {
            return new MatchModel
            {
                MatchDateTime = "2018-10-15 9:00:00PM",
                MatchTitle = "Bayern vs Wolfsburg"
            };
        }

        MatchModel ReturnInvalidDemoMatch()
        {
            return new MatchModel();
        }
    }
}