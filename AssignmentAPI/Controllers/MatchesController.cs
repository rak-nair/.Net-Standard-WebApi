using AssignmentAPI.Models;
using AssignmentAPI.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace AssignmentAPI.Controllers
{
    public class MatchesController : BaseApiController
    {
        public MatchesController(IAssignmentData data) : base(data)
        {

        }

        [HttpGet]
        public IHttpActionResult GetAllMatches(int page = 1, int pageSize = 50)
        {
            try
            {
                //It'll be prudent to set an upper limit for pageSize.
                var matches = TheRepository.GetAllMatches();
                var total = matches.Count();
                var pagedResults = matches
                                    .Skip((page - 1) * pageSize)
                                    .Take(pageSize).ToList();
                var paging = CreatePageLinks(Url, "Matches", null, page, pageSize, total);

                return Ok(new PagedMatchViewModel
                {
                    Matches = pagedResults,
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

        [HttpPost]
        public async Task<IHttpActionResult> AddMatch([FromBody] MatchModel match)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var matchEntity = TheModelFactory.Parse(match);
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

        [HttpGet]
        [Route("api/matches/{matchid}/players")]
        public IHttpActionResult GetPlayersInMatch(int matchid, int page = 1, int pageSize = 50)
        {
            try
            {
                //It'll be prudent to set an upper limit for pageSize.
                var playersInMatchEntities = TheRepository.GetMatchPlayersInMatch(matchid);
                var playersInMatch = TheModelFactory.Create(playersInMatchEntities);
                var total = playersInMatch.Count();
                var pagedResults = playersInMatch
                                    .Skip((page - 1) * pageSize)
                                    .Take(pageSize).ToList();
                var paging = CreatePageLinks(Url, "PlayersInMatch", null, page, pageSize, total);

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
    }
}