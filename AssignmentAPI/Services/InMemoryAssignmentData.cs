using AssignmentAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssignmentAPI.Services
{
    //In Memory Datastore for tests.
    public class InMemoryAssignmentData : IAssignmentData
    {
        #region Private_Variables
        private IEnumerable<MatchEntity> _matches;
        private IEnumerable<PlayerEntity> _players;
        private IEnumerable<MatchPlayerEntity> _matchPlayers;
        #endregion

        #region Properties
        //The Samples are used in the Unit Test.
        public MatchEntity SampleMatch { get; private set; }
        public PlayerEntity SamplePlayer { get; private set; }
        #endregion

        #region Constructor
        public InMemoryAssignmentData()
        {
            PlayerEntity player1 = new PlayerEntity { PlayerID = 1, Name = "Lionel Messi", YearOfBirth = 1980 };
            PlayerEntity player2 = new PlayerEntity { PlayerID = 2, Name = "C Ronaldo", YearOfBirth = 1978 };
            PlayerEntity player3 = new PlayerEntity { PlayerID = 3, Name = "Neymar", YearOfBirth = 1984 };
            PlayerEntity player4 = new PlayerEntity { PlayerID = 4, Name = "L Suarez", YearOfBirth = 1983 };

            _players = new List<PlayerEntity> { player1, player2, player3, player4 };

            MatchEntity match1 = new MatchEntity
            {
                MatchID = 1,
                MatchTitle = "FC Kron vs FC Wolfsburg",
                MatchDateTime = new DateTime(2018, 10, 15, 20, 00, 00)
            };
            MatchEntity match2 = new MatchEntity
            {
                MatchID = 2,
                MatchTitle = "FC Barcelona vs FC Rela Madrid",
                MatchDateTime = new DateTime(2018, 10, 16, 20, 00, 00)
            };

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
        #endregion

        #region Players
        public async Task<(List<PlayerEntity> Players, int TotalRows)> GetAllPlayers(int page, int pageSize)
        {
            var totalPlayers = _players.Count();
            var players = ReturnPagedData(_players.OrderBy(x => x.PlayerID), page, pageSize);
            var result = (players, totalPlayers);

            return await Task.FromResult(result);
        }

        public async Task<PlayerEntity> GetPlayer(int playerId)
        {
            return await Task.FromResult(_players
                                    .Where(x => x.PlayerID == playerId)
                                    .FirstOrDefault());
        }

        public Task<bool> DoesPlayerExist(string name, int yearOfBirth)
        {
            return Task.FromResult(_players
                                    .Any(x => x.Name == name
                                        && x.YearOfBirth == yearOfBirth));
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
        #endregion

        #region Matches
        public async Task<(List<MatchEntity> Matches, int TotalRows)> GetAllMatches(int page, int pageSize)
        {
            var totalMatches = _matches.Count();
            var matches = ReturnPagedData(_matches.OrderBy(x => x.MatchID), page, pageSize);

            var result = (matches, totalMatches);

            return await Task.FromResult(result);
        }

        public async Task<MatchEntity> GetMatch(int matchId)
        {
            return await Task.FromResult(_matches.Where(x => x.MatchID == matchId).FirstOrDefault());
        }

        public async Task<bool> DoesMatchExist(string matchTitle, DateTime matchDateTime)
        {
            return await Task.FromResult(_matches
                                    .Any(x => x.MatchTitle == matchTitle
                                        && x.MatchDateTime == matchDateTime));
        }

        public async Task<bool> DoesMatchExist(int matchID)
        {
            return await Task.FromResult(_matches
                                    .Any(x => x.MatchID == matchID));
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
        #endregion

        #region MatchPlayers
        public async Task<(List<MatchPlayerEntity> MatchPlayers, int TotalRows)> GetMatchPlayersInMatch
                (int matchId, int page, int pageSize)
        {
            var matchPlayers = ReturnPagedData(_matchPlayers
                                .Where(x => x.Match.MatchID == matchId)
                                .OrderBy(x => x.MatchPlayerID), page, pageSize);

            var totalMatchPlayers = matchPlayers.Count();
            var result = (matchPlayers, totalMatchPlayers);

            return await Task.FromResult(result);
        }

        public async Task<bool> DoesMatchPlayerExist(int matchID, int playerID)
        {
            return await Task.FromResult(_matchPlayers
                                    .Any(x => x.Match.MatchID == matchID
                                        && x.Player.PlayerID == playerID));
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
        #endregion

        #region Helper Methods
        List<T> ReturnPagedData<T>(IOrderedEnumerable<T> input, int page, int pageSize)
        {
            return input
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
        }
        #endregion
    }
}