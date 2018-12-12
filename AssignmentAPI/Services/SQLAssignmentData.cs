using AssignmentAPI.Data;
using AssignmentAPI.Data.Entities;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;

namespace AssignmentAPI.Services
{
    public class SQLAssignmentData : IAssignmentData
    {
        private AssignmentDbContext _context;

        public SQLAssignmentData(AssignmentDbContext context)
        {
            _context = context;
        }

        public MatchEntity AddMatch(MatchEntity match)
        {
            try
            {
                _context.Matches.Add(match);
                _context.SaveChanges();
                return match;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PlayerEntity AddPlayer(PlayerEntity player)
        {
            try
            {
                _context.Players.Add(player);
                _context.SaveChanges();
                return player;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public MatchPlayerEntity AddPlayerToMatch(MatchPlayerEntity matchPlayer)
        {
            try
            {
                _context.Matches.Attach(matchPlayer.Match);
                _context.Players.Attach(matchPlayer.Player);

                _context.MatchPlayers.Add(matchPlayer);
                _context.SaveChanges();
                return matchPlayer;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public IEnumerable<MatchEntity> GetAllMatches()
        {
            //https://stackoverflow.com/questions/38752848/paging-the-huge-data-that-is-returned-by-the-web-api

            return _context.Matches.AsNoTracking().ToList();
        }

        public IEnumerable<PlayerEntity> GetAllPlayers()
        {
            return _context.Players.AsNoTracking().ToList();
        }

        public MatchEntity GetMatch(int matchId)
        {
            return _context.Matches.AsNoTracking().AsQueryable().Where(x => x.MatchID == matchId).FirstOrDefault();
        }

        public PlayerEntity GetPlayer(int playerId)
        {
            return _context.Players.AsNoTracking().AsQueryable().Where(x => x.PlayerID == playerId).FirstOrDefault();
        }

        public IEnumerable<MatchPlayerEntity> GetMatchPlayersInMatch(int matchId)
        {
            //var players = _context.MatchPlayers.ToList();
            //var foo = players.Where(x => x.Match.MatchID == matchId);
            //return foo;
            _context.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
            var players = _context.MatchPlayers.AsNoTracking().Include(x => x.Match).Include(x => x.Player).AsQueryable().Where(x => x.Match.MatchID == matchId);
            return players;
        } 
    }
}