using AssignmentAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssignmentAPI.Services
{
    public class InMemoryAssignmentData : IAssignmentData
    {
        private IEnumerable<MatchEntity> _matches;
        private IEnumerable<PlayerEntity> _players;
        private IEnumerable<MatchPlayerEntity> _matchPlayers;

        public MatchEntity SampleMatch { get; private set; }
        public PlayerEntity SamplePlayer { get; private set; }

        public InMemoryAssignmentData()
        {
            PlayerEntity player1 = new PlayerEntity { PlayerID = 1, Name = "Lionel Messi", YearOfBirth = 1980 };
            PlayerEntity player2 = new PlayerEntity { PlayerID = 2, Name = "C Ronaldo", YearOfBirth = 1978 };
            PlayerEntity player3 = new PlayerEntity { PlayerID = 3, Name = "Neymar", YearOfBirth = 1984 };
            PlayerEntity player4 = new PlayerEntity { PlayerID = 4, Name = "L Suarez", YearOfBirth = 1983 };

            _players = new List<PlayerEntity> { player1, player2, player3, player4 };

            MatchEntity match1 = new MatchEntity { MatchID = 1, MatchTitle = "FC Kron vs FC Wolfsburg", MatchDateTime = new DateTime(2018, 10, 15, 20, 00, 00) };
            MatchEntity match2 = new MatchEntity { MatchID = 2, MatchTitle = "FC Barcelona vs FC Rela Madrid", MatchDateTime = new DateTime(2018, 10, 16, 20, 00, 00) };

            _matches = new List<MatchEntity> { match1, match2 };

            _matchPlayers = new List<MatchPlayerEntity> {
                new MatchPlayerEntity { MatchPlayerID = 1, Match = match1, Player = player1 },
                new MatchPlayerEntity { MatchPlayerID = 2, Match = match1, Player = player2 },
                new MatchPlayerEntity { MatchPlayerID = 3, Match = match2, Player = player3 },
                new MatchPlayerEntity { MatchPlayerID = 4, Match = match2, Player = player4 },
            };

            SamplePlayer = player1;
            SampleMatch = match1;
        }

        public async Task<MatchEntity> AddMatchAsync(MatchEntity match)
        {
            try
            {
                match.MatchID = _matches.Max(x => x.MatchID) + 1;
                _matches = _matches.Concat(new[] { match });
                return await Task.FromResult(match);
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
                player.PlayerID = _players.Max(x => x.PlayerID) + 1;
                _players = _players.Concat(new[] { player });
                return await Task.FromResult(player);
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
                matchPlayer.MatchPlayerID = _matchPlayers.Max(x => x.MatchPlayerID) + 1;
                _matchPlayers = _matchPlayers.Concat(new[] { matchPlayer });
                return await Task.FromResult(matchPlayer);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IQueryable<MatchEntity> GetAllMatches()
        {
            return _matches.AsQueryable().OrderBy(x=>x.MatchID);
        }

        
        public IQueryable<PlayerEntity> GetAllPlayers()
        {
            return _players.AsQueryable().OrderBy(x=>x.PlayerID);
        }

        public Task<MatchEntity> GetMatch(int matchId)
        {
            return Task.FromResult(_matches.Where(x => x.MatchID == matchId).FirstOrDefault());
        }

        public Task<PlayerEntity> GetPlayer(int playerId)
        {
            return Task.FromResult(_players.Where(x => x.PlayerID == playerId).FirstOrDefault());
        }

        public IQueryable<MatchPlayerEntity> GetMatchPlayersInMatch(int matchId)
        {
            if (!_matches.Any(x => x.MatchID == matchId))
                throw new Exception("No such match exists.");
            return _matchPlayers.Where(x => x.Match.MatchID == matchId).AsQueryable();
        }
    
    }
}