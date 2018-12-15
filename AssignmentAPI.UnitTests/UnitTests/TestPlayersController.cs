using AssignmentAPI.Controllers;
using AssignmentAPI.Data.Entities;
using AssignmentAPI.Models;
using AssignmentAPI.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace AssignmentAPI.UnitTests
{
    [TestClass]
    public class TestPlayersController : ControllerTestSetUpBase
    {
        [TestMethod]
        public void GetAllPlayers_ShouldReturnAllPlayers()
        {
            var sut = new PlayersController(new InMemoryAssignmentData());
            sut = SetUpDummyPaging(sut, "Players") as PlayersController;

            var result = sut.GetAllPlayers() as OkNegotiatedContentResult<PagedPlayerViewModel>;

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Content.Players.Count());
        }

        [TestMethod]
        public async Task GetPlayer_ValidID_Success()
        {
            var sut = new PlayersController(new InMemoryAssignmentData());

            var result = await sut.GetPlayer(1) as OkNegotiatedContentResult<PlayerEntity>;

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Content.PlayerID);
        }

        [TestMethod]
        public async Task GetPlayer_InvalidID_BadRequest()
        {
            var sut = new PlayersController(new InMemoryAssignmentData());

            var result = await sut.GetPlayer(99);

            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
            Assert.AreEqual(((BadRequestErrorMessageResult)result).Message, new ErrorResponses().NO_PLAYER_EXISTS);
        }

        [TestMethod]
        public async Task AddPlayer_ValidData_Success()
        {
            var sut = new PlayersController(new InMemoryAssignmentData());
            PlayerModel demoPlayer = ReturnValidDemoPlayer();

            var result = await sut.AddPlayer(demoPlayer) as CreatedAtRouteNegotiatedContentResult<PlayerEntity>;

            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.RouteValues["playerid"]);
        }

        [TestMethod]
        public void AddPlayer_InvalidData_BadModelState()
        {
            var sut = new PlayersController(new InMemoryAssignmentData());
            PlayerModel demoPlayer = ReturnInvalidDemoPlayer();
            sut = SetUpDummyHttpConfiguration(sut) as PlayersController;

            sut.Validate(demoPlayer);

            Assert.IsFalse(sut.ModelState.IsValid);
            Assert.AreEqual(2, sut.ModelState.Keys.Count);
        }

        [TestMethod]
        public async Task AddPlayer_Duplicate_BadRequest()
        {
            var data = new InMemoryAssignmentData()
            {
                ErrorResposnses = new ErrorResponses()
            };
            var sut = new PlayersController(data);
            var demoMatch = new PlayerModel
            {
                Name = data.SamplePlayer.Name,
                YearOfBirth = data.SamplePlayer.YearOfBirth
            };

            var result = await sut.AddPlayer(demoMatch);

            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
            Assert.AreEqual(((BadRequestErrorMessageResult)result).Message, new ErrorResponses().DUPLICATE_PLAYER);
        }

        PlayerModel ReturnValidDemoPlayer()
        {
            return new PlayerModel
            {
                Name = "Ibrahimovic",
                YearOfBirth = 1985
            };
        }

        PlayerModel ReturnInvalidDemoPlayer()
        {
            return new PlayerModel();
        }
    }
}