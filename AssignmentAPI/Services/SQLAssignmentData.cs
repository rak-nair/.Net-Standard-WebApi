using AssignmentAPI.Data;
using AssignmentAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace AssignmentAPI.Services
{
    //SQL Datastore.
    public class SQLAssignmentData : IAssignmentData
    {
        #region Private_Variables
        private AssignmentDbContext _context;
        #endregion

        #region Constructor
        public SQLAssignmentData(AssignmentDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Players
        public async Task<(List<PlayerEntity> Players, int TotalRows)> GetAllPlayers(int page, int pageSize)
        {
            try
            {
                var players = _context.Players.AsNoTracking();
                var totalPlayers = await players.CountAsync();
                var pagedResults = await ReturnPagedData(players
                                                            .OrderBy(x => x.PlayerID), page, pageSize);

                return (pagedResults, totalPlayers);
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

        public async Task<bool> DoesPlayerExist(string name, int yearOfBirth)
        {
            try
            {
                return await _context.Players
                                .AsNoTracking()
                                .AnyAsync(x => x.Name == name && x.YearOfBirth == yearOfBirth);
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
        #endregion

        #region Matches
        public async Task<(List<MatchEntity> Matches, int TotalRows)> GetAllMatches(int page, int pageSize)
        {
            try
            {
                var matches = _context.Matches.AsNoTracking();
                var totalMatchess = await matches.CountAsync();
                var pagedResults = await ReturnPagedData(matches
                                                            .OrderBy(x => x.MatchID), page, pageSize);

                return (pagedResults, totalMatchess);
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

        public async Task<bool> DoesMatchExist(string matchTitle, DateTime matchDateTime)
        {
            try
            {
                return await _context.Matches
                                .AsNoTracking()
                                .AnyAsync(x => x.MatchTitle == matchTitle && x.MatchDateTime == matchDateTime);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DoesMatchExist(int matchID)
        {
            try
            {
                return await _context.Matches
                                .AsNoTracking()
                                .AnyAsync(x => x.MatchID == matchID);
            }
            catch (Exception)
            {
                throw;
            }
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
        #endregion

        #region MatchPlayer
        public async Task<(List<MatchPlayerEntity> MatchPlayers, int TotalRows)> GetMatchPlayersInMatch
                                    (int matchId, int page, int pageSize)
        {
            try
            {
                var matchPlayers = _context.MatchPlayers
                                            .AsNoTracking()
                                            .Where(x => x.Match.MatchID == matchId);
                var totalMatchPlayers = await matchPlayers.CountAsync();
                var pagedResults = await ReturnPagedData(matchPlayers
                                                            .Include(x => x.Player)
                                                            .OrderBy(x => x.MatchPlayerID), page, pageSize);

                return (pagedResults, totalMatchPlayers);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DoesMatchPlayerExist(int matchID, int playerID)
        {
            try
            {
                return await _context.MatchPlayers
                            .AsNoTracking()
                            .Where(x => x.Match.MatchID == matchID)
                            .AnyAsync(x => x.Player.PlayerID == playerID);
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
        #endregion

        #region Helper Methods
        async Task<List<T>> ReturnPagedData<T>(IQueryable<T> input, int page, int pageSize)
        {
            return await input
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
        }
        #endregion
    }
}