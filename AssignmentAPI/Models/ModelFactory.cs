using AssignmentAPI.Data.Entities;
using AssignmentAPI.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AssignmentAPI.Models
{
    //Class that converts between Entites and Models.
    //Parse methods convert from Model to Entity and Create methods go the other way.
    public class ModelFactory
    {
        private IAssignmentData _data;
        private ErrorResponses _responses;

        public ModelFactory(IAssignmentData data, ErrorResponses responses)
        {
            _data = data;
            _responses = responses;
        }
        public PlayerEntity Parse(PlayerModel playerModel)
        {
            var playerEntity = new PlayerEntity
            {
                Name = playerModel.Name,
                YearOfBirth = playerModel.YearOfBirth
            };
            if (_data.GetAllPlayers().Any(x => x.Name == playerEntity.Name
                                            && x.YearOfBirth == playerEntity.YearOfBirth))
            {
                throw new Exception(_responses.DUPLICATE_PLAYER);
            }

            return playerEntity;
        }

        public MatchEntity Parse(MatchModel matchModel)
        {
            var matchEntity = new MatchEntity
            {
                MatchDateTime = DateTime.Parse(matchModel.MatchDateTime),
                MatchTitle = matchModel.MatchTitle
            };

            if (_data.GetAllMatches().Any
                        (x => x.MatchDateTime == matchEntity.MatchDateTime
                            && x.MatchTitle == matchEntity.MatchTitle))
            {
                throw new Exception(_responses.DUPLICATE_MATCH);
            }

            return matchEntity;
        }

        public async Task<MatchPlayerEntity> Parse(int matchId, int playerID)
        {
            try
            {
                var match = await _data.GetMatch(matchId);
                if (match == null)
                    throw new Exception(_responses.NO_MATCH_EXISTS);

                var player = await _data.GetPlayer(playerID);
                if (player == null)
                    throw new Exception(_responses.NO_PLAYER_EXISTS);

                var matchPlayers = _data.GetMatchPlayersInMatch(matchId);

                if (matchPlayers.Any(x => x.Player.PlayerID == playerID))
                {
                    throw new Exception(_responses.DUPLICATE_PLAYER_IN_MATCH);
                }

                return new MatchPlayerEntity { Match = match, Player = player };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IQueryable<MatchPlayerModel> Create(IQueryable<MatchPlayerEntity> matchPlayer)
        {
            return matchPlayer
                .Select(x =>
                    new MatchPlayerModel
                    {
                        MatchPlayerID = x.MatchPlayerID,
                        Player = x.Player
                    })
                    .OrderBy(x => x.MatchPlayerID);
        }
    }
}