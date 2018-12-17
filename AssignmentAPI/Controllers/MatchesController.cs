using AssignmentAPI.Models;
using AssignmentAPI.Services;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace AssignmentAPI.Controllers
{
    public class MatchesController : BaseApiController
    {
        public MatchesController(IAssignmentData data) : base(data)
        {

        }

        #region GetMethods
        [HttpGet]
        public async Task<IHttpActionResult> GetAllMatches(int page = 1, int pageSize = 50)
        {
            try
            {
                //It'll be prodent to set an upper limit for pageSize.
                var pagedResults = await TheRepository.GetAllMatches(page, pageSize);
                var paging = CreatePageLinks(Url, "Matches", null, page, pageSize, pagedResults.TotalRows);

                return Ok(new PagedMatchViewModel
                {
                    Matches = pagedResults.Matches,
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
        public async Task<IHttpActionResult> GetMatch(int matchid)
        {
            try
            {
                var match = await TheRepository.GetMatch(matchid);

                if (match == null)
                    return BadRequest(TheErrorResponses.NO_MATCH_EXISTS);
                else
                    return Ok(match);
            }
            catch (Exception ex)
            {
                return BadRequest($"No data - {ex.Message}");
            }
        }

        [HttpGet]
        [Route("api/matches/{matchid}/players")]
        public async Task<IHttpActionResult> GetPlayersInMatch(int matchid, int page = 1, int pageSize = 50)
        {
            try
            {
                if (!await TheRepository.DoesMatchExist(matchid))
                {
                    return BadRequest(TheErrorResponses.NO_MATCH_EXISTS);
                }
                //It'll be prudent to set an upper limit for pageSize.
                var resultEntities = await TheRepository.GetMatchPlayersInMatch(matchid, page, pageSize);
                var pagedResults = TheModelFactory.Create(resultEntities.MatchPlayers);
                var paging = CreatePageLinks(Url, "PlayersInMatch", null, page, pageSize, resultEntities.TotalRows);

                return Ok(new PagedMatchPlayerViewModel
                {
                    MatchPlayers = pagedResults,
                    Pages = paging
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region PostMethods
        [HttpPost]
        public async Task<IHttpActionResult> AddMatch([FromBody] MatchModel match)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var matchEntity = await TheModelFactory.Parse(match);
                    matchEntity = await TheRepository.AddMatchAsync(matchEntity);

                    return CreatedAtRoute("Matches", new { matchid = matchEntity.MatchID }, matchEntity);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else
                return BadRequest(ModelState);
        }

        [HttpPost]
        [Route("api/matches/{matchid}/players/{matchPlayerid?}", Name = "PlayersInMatch")]
        public async Task<IHttpActionResult> AddPlayerToMatch(int matchId, [FromBody] int playerID)
        {
            try
            {
                var playerInMatch = await TheModelFactory.Parse(matchId, playerID);
                playerInMatch = await TheRepository.AddPlayerToMatch(playerInMatch);

                return CreatedAtRoute("PlayersInMatch",
                    new { matchid = matchId, matchPlayerid = playerInMatch.MatchPlayerID },
                    playerInMatch);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}