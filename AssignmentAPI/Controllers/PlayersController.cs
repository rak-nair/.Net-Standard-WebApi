using AssignmentAPI.Models;
using AssignmentAPI.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace AssignmentAPI.Controllers
{
    public class PlayersController : BaseApiController
    {
        public PlayersController(IAssignmentData data) : base(data)
        {

        }

        [HttpGet]
        public IHttpActionResult GetAllPlayers(int page = 1, int pageSize = 50)
        {
            try
            {
                var players = TheRepository.GetAllPlayers()
                                    .Skip((page - 1) * pageSize)
                                    .Take(pageSize).ToList();

                return Ok(players);
            }
            catch (Exception ex)
            {
                //safer to do with a custom exception, so as to not
                //accidentally display sensitive information.
                return BadRequest($"No data - {ex.Message}");
            }

        }

        [HttpGet]
        public async Task<IHttpActionResult> GetPlayer(int playerid)
        {
            try
            {
                var player = await TheRepository.GetPlayer(playerid);

                if (player == null)
                {
                    return BadRequest("No such player exists");
                }
                else
                {
                    return Ok(player);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"No data - {ex.Message}");
            }

        }

        [HttpPost]
        public async Task<IHttpActionResult> AddPlayer([FromBody]PlayerModel player)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var playerEntity = TheModelFactory.Parse(player);
                    if (TheRepository.GetAllPlayers().Any(x => x.Name == playerEntity.Name && x.YearOfBirth == playerEntity.YearOfBirth))
                    {
                        return BadRequest("Player already exists");
                    }

                    playerEntity = await TheRepository.AddPlayer(playerEntity);

                    return CreatedAtRoute("Players", new { playerid = playerEntity.PlayerID }, playerEntity);

                }
                catch (Exception ex)
                {
                    return BadRequest($"Failed to add a new player - {ex.Message}");
                }
            }
            return BadRequest(ModelState);
        }
    }
}