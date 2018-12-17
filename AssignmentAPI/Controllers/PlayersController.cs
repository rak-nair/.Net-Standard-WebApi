using AssignmentAPI.Models;
using AssignmentAPI.Services;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace AssignmentAPI.Controllers
{
    public class PlayersController : BaseApiController
    {
        public PlayersController(IAssignmentData data) : base(data)
        {

        }

        #region GetMethods
        [HttpGet]
        public async Task<IHttpActionResult> GetAllPlayers(int page = 1, int pageSize = 50)
        {
            try
            {
                //It'll be prodent to set an upper limit for pageSize.
                var pagedResults = await TheRepository.GetAllPlayers(page,pageSize);
                var paging = CreatePageLinks(Url, "Players", null, page, pageSize,pagedResults.TotalRows);

                return Ok(new PagedPlayerViewModel
                {
                    Players = pagedResults.Players,
                    Pages = paging
                });
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
                    return BadRequest(TheErrorResponses.NO_PLAYER_EXISTS);
                }
                else
                {
                    return Ok(player);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        #endregion

        #region PostMethods
        [HttpPost]
        public async Task<IHttpActionResult> AddPlayer([FromBody]PlayerModel player)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var playerEntity = await TheModelFactory.Parse(player);
                    playerEntity = await TheRepository.AddPlayer(playerEntity);

                    return CreatedAtRoute("Players", new { playerid = playerEntity.PlayerID }, playerEntity);

                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest(ModelState);
        }
        #endregion
    }
}