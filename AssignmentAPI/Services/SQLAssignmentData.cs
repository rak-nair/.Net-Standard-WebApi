using AssignmentAPI.Data;
using AssignmentAPI.Data.Entities;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssignmentAPI.Services
{
    public class SQLAssignmentData : IAssignmentData
    {
        private AssignmentDbContext _context;

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
            //https://stackoverflow.com/questions/38752848/paging-the-huge-data-that-is-returned-by-the-web-api
            return _context.Matches.AsNoTracking().AsQueryable().OrderBy(x=>x.MatchID);
        }

        public IQueryable<PlayerEntity> GetAllPlayers()
        {
            return _context.Players.AsNoTracking().OrderBy(x=>x.PlayerID);
        }

        public Task<MatchEntity> GetMatch(int matchId)
        {
            return _context.Matches.AsNoTracking().FirstOrDefaultAsync(x => x.MatchID == matchId);
        }

        public async Task<PlayerEntity> GetPlayer(int playerId)
        {
            return await _context.Players.AsNoTracking().FirstOrDefaultAsync(x => x.PlayerID == playerId);
        }

        public IQueryable<MatchPlayerEntity> GetMatchPlayersInMatch(int matchId)
        {
            return _context.MatchPlayers.AsNoTracking().Include(x => x.Match).Include(x => x.Player).Where(x => x.Match.MatchID == matchId);
        }
        
    }
}