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
                var total = TheRepository.GetAllMatches().Count();
                var matches = TheRepository.GetAllMatches()
                                    .Skip((page - 1) * pageSize)
                                    .Take(pageSize).ToList();
                var paging = CreatePageLinks(Url, "Matches", null, page, pageSize, total);

                return Ok(new PagedMatchViewModel
                {
                    Matches = matches,
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
                    return BadRequest("No such match exists");
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
                    if (TheRepository.GetAllMatches().Any
                        (x => x.MatchDateTime == matchEntity.MatchDateTime && x.MatchTitle == matchEntity.MatchTitle))
                    {
                        return BadRequest("Match already exists");
                    }

                    matchEntity = await TheRepository.AddMatchAsync(matchEntity);

                    return CreatedAtRoute("Matches", new { matchid = matchEntity.MatchID }, matchEntity);
                }
                catch (Exception ex)
                {
                    return BadRequest($"Failed to add a new match - {ex.Message}");
                }
            }
            else
                return BadRequest(ModelState);
        }

        [HttpGet]
        [Route("api/matches/{matchid}/players")]
        public IHttpActionResult GetPlayersInMatch(int matchid)
        {
            try
            {
                var playersInMatch = TheModelFactory.Create(TheRepository.GetMatchPlayersInMatch(matchid));

                return Ok(playersInMatch.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest($"No data - {ex.Message}");
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
                return BadRequest($"Failed to add player - {ex.Message}");
            }
        }
    }
}