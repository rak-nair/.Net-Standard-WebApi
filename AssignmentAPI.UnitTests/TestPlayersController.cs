using AssignmentAPI.Controllers;
using AssignmentAPI.Data.Entities;
using AssignmentAPI.Models;
using AssignmentAPI.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace AssignmentAPI.UnitTests
{
    [TestClass]
    public class TestPlayersController
    {
        [TestMethod]
        public void GetAllPlayers_ShouldReturnAllPlayers()
        {
            var sut = new PlayersController(new InMemoryAssignmentData());

            var result = sut.GetAllPlayers() as OkNegotiatedContentResult<IEnumerable<PlayerEntity>>;

            Assert.IsNotNull(result);
            Assert.AreEqual(4, ((List<PlayerEntity>)result.Content).Count);
        }

        [TestMethod]
        public void GetPlayer_ValidID_Success()
        {
            var sut = new PlayersController(new InMemoryAssignmentData());

            var result = sut.GetPlayer(1) as OkNegotiatedContentResult<PlayerEntity>;

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Content.PlayerID);
        }

        [TestMethod]
        public void GetPlayer_InvalidID_BadRequest()
        {
            var sut = new PlayersController(new InMemoryAssignmentData());

            var result = sut.GetPlayer(99);

            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
        }

        [TestMethod]
        public void AddPlayer_ValidData_Success()
        {
            var sut = new PlayersController(new InMemoryAssignmentData());
            PlayerModel demoPlayer = ReturnValidDemoPlayer();

            var result = sut.AddPlayer(demoPlayer) as CreatedAtRouteNegotiatedContentResult<PlayerEntity>;

            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.RouteValues["playerid"]);
        }

        [TestMethod]
        public void AddPlayer_InvalidData_BadModelState()
        {
            var sut = new PlayersController(new InMemoryAssignmentData());
            PlayerModel demoPlayer = ReturnInvalidDemoPlayer();
            sut.Request = new HttpRequestMessage();
            sut.Request.Properties["MS_HttpConfiguration"] = new HttpConfiguration();

            sut.Validate(demoPlayer);

            Assert.IsFalse(sut.ModelState.IsValid);
            Assert.AreEqual(2, sut.ModelState.Keys.Count);
        }

        [TestMethod]
        public void AddPlayer_Duplicate_BadRequest()
        {
            var data = new InMemoryAssignmentData();
            var sut = new PlayersController(data);
            var demoMatch = new PlayerModel
            {
                Name = data.SamplePlayer.Name,
                YearOfBirth = data.SamplePlayer.YearOfBirth
            };

            var result = sut.AddPlayer(demoMatch);

            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
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