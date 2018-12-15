using AssignmentAPI.Data;
using AssignmentAPI.Data.Entities;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace AssignmentAPI.Services
{
    //SQL Datastore.
    public class SQLAssignmentData : IAssignmentData
    {
        private AssignmentDbContext _context;

        public ErrorResponses ErrorResposnses { get; set; }

        public SQLAssignmentData(AssignmentDbContext context)
        {
            _context = context;
        }

        public async Task<MatchEntity> AddMatchAsync(MatchEntity match)
        {
            try
            {
                _context.Matches.Add(match);
                await _context.SaveChangesAsync();
                return match;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PlayerEntity> AddPlayer(PlayerEntity player)
        {
            try
            {
                _context.Players.Add(player);
                await _context.SaveChangesAsync();
                return player;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<MatchPlayerEntity> AddPlayerToMatch(MatchPlayerEntity matchPlayer)
        {
            try
            {
                //Attach since everything is retrieved AsNoTracking.
                _context.Matches.Attach(matchPlayer.Match);
                _context.Players.Attach(matchPlayer.Player);

                _context.MatchPlayers.Add(matchPlayer);
                await _context.SaveChangesAsync();
                return matchPlayer;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public IQueryable<MatchEntity> GetAllMatches()
        {
            try
            {
                //AsNoTracking is used to improve performance.
                return _context.Matches.AsNoTracking().OrderBy(x => x.MatchID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IQueryable<PlayerEntity> GetAllPlayers()
        {
            try
            {
                return _context.Players.AsNoTracking().OrderBy(x => x.PlayerID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<MatchEntity> GetMatch(int matchId)
        {
            try
            {
                return _context.Matches.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.MatchID == matchId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PlayerEntity> GetPlayer(int playerId)
        {
            try
            {
                return await _context.Players.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.PlayerID == playerId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IQueryable<MatchPlayerEntity> GetMatchPlayersInMatch(int matchId)
        {
            try
            {
                if (!_context.Matches.Any(x => x.MatchID == matchId))
                    throw new Exception(ErrorResposnses.NO_MATCH_EXISTS);

                return _context.MatchPlayers.AsNoTracking()
                    .Include(x => x.Match)
                    .Include(x => x.Player)
                    .Where(x => x.Match.MatchID == matchId);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}