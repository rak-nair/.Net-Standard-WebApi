using AssignmentAPI.Controllers;
using AssignmentAPI.Data.Entities;
using AssignmentAPI.Models;
using AssignmentAPI.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace AssignmentAPI.UnitTests
{
    [TestClass]
    public class TestMatchesController : ControllerTestSetUpBase
    {
        #region GetAllMatches
        [TestMethod]
        public async Task GetAllMatches_ShouldReturnAllMatches()
        {
            var sut = new MatchesController(new InMemoryAssignmentData());
            sut = SetUpDummyPaging(sut, "Matches") as MatchesController;

            var result = await sut.GetAllMatches()
                as OkNegotiatedContentResult<PagedMatchViewModel>;

            Assert.IsNotNull(result.Content);
            Assert.AreEqual(2, result.Content.Matches.Count);
        }

        [TestMethod]
        public async Task GetAllMatches_Paged_ShouldReturnAllMatchesByPageSize()
        {
            var sut = new MatchesController(new InMemoryAssignmentData());
            sut = SetUpDummyPaging(sut, "Matches") as MatchesController;

            var result = await sut.GetAllMatches(1, 1)
                as OkNegotiatedContentResult<PagedMatchViewModel>;

            Assert.IsNotNull(result.Content);
            Assert.AreEqual(1, result.Content.Matches.Count);
        }
        #endregion

        #region GetMatch
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
            Assert.AreEqual(((BadRequestErrorMessageResult)result).Message, new ErrorResponses().NO_MATCH_EXISTS);
        }
        #endregion

        #region AddMatch
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
            sut = SetUpDummyHttpConfiguration(sut) as MatchesController;

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
            Assert.AreEqual(((BadRequestErrorMessageResult)result).Message, new ErrorResponses().DUPLICATE_MATCH);
        }
        #endregion

        #region GetPlayersInMatch
        [TestMethod]
        public async Task GetPlayersInMatch_ValidID_ShouldReturnAllPlayers()
        {
            var sut = new MatchesController(new InMemoryAssignmentData());
            sut = SetUpDummyPaging(sut, "PlayersInMatch") as MatchesController;

            var result = await sut.GetPlayersInMatch(1)
                as OkNegotiatedContentResult<PagedMatchPlayerViewModel>;

            Assert.IsNotNull(result.Content);
            Assert.AreEqual(2, result.Content.MatchPlayers.Count);
            Assert.AreEqual(1, result.Content.MatchPlayers[0].Player.PlayerID);
        }

        [TestMethod]
        public async Task GetPlayersInMatch_Paged_ValidID_ShouldReturnAllPlayersByPageSize()
        {
            var sut = new MatchesController(new InMemoryAssignmentData());
            sut = SetUpDummyPaging(sut, "PlayersInMatch") as MatchesController;

            var result = await sut.GetPlayersInMatch(1,1,1)
                as OkNegotiatedContentResult<PagedMatchPlayerViewModel>;

            Assert.IsNotNull(result.Content);
            Assert.AreEqual(1, result.Content.MatchPlayers.Count);
            Assert.AreEqual(1, result.Content.MatchPlayers[0].Player.PlayerID);
        }

        [TestMethod]
        public async Task GetPlayersInMatch_InvalidID_BadRequest()
        {
            var sut = new MatchesController(new InMemoryAssignmentData());

            var result = await sut.GetPlayersInMatch(99);

            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
            Assert.AreEqual(((BadRequestErrorMessageResult)result).Message, new ErrorResponses().NO_MATCH_EXISTS);
        }
        #endregion

        #region AddPlayerToMatch
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
            Assert.AreEqual(((BadRequestErrorMessageResult)result).Message, new ErrorResponses().NO_PLAYER_EXISTS);
        }

        [TestMethod]
        public async Task AddPlayerToMatch_InvalidData_MatchID_BadRequest()
        {
            var sut = new MatchesController(new InMemoryAssignmentData());

            var result = await sut.AddPlayerToMatch(9, 9);

            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
            Assert.AreEqual(((BadRequestErrorMessageResult)result).Message, new ErrorResponses().NO_MATCH_EXISTS);
        }

        [TestMethod]
        public async Task AddPlayerToMatch_Duplicate_BadRequest()
        {
            var sut = new MatchesController(new InMemoryAssignmentData());

            var result = await sut.AddPlayerToMatch(1, 1);

            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
            Assert.AreEqual(((BadRequestErrorMessageResult)result).Message, new ErrorResponses().DUPLICATE_PLAYER_IN_MATCH);
        }
        #endregion

        #region DummyDemoMatches
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
        #endregion
    }
}