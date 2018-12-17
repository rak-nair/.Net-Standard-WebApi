using AssignmentAPI.Data.Entities;
using AssignmentAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssignmentAPI.Models
{
    //Class that converts between Entites and Models.
    //Parse methods convert from Model to Entity and Create methods go the other way.
    public class ModelFactory
    {
        #region Private_Variables
        private IAssignmentData _data;
        private ErrorResponses _responses;
        #endregion

        #region Constructor
        public ModelFactory(IAssignmentData data, ErrorResponses responses)
        {
            _data = data;
            _responses = responses;
        }
        #endregion

        #region Parse - Get Entity From Model
        public async Task<PlayerEntity> Parse(PlayerModel playerModel)
        {
            var playerEntity = new PlayerEntity
            {
                Name = playerModel.Name,
                YearOfBirth = playerModel.YearOfBirth
            };
            if (await _data.DoesPlayerExist(playerEntity.Name, playerEntity.YearOfBirth))
            {
                throw new Exception(_responses.DUPLICATE_PLAYER);
            }

            return playerEntity;
        }

        public async Task<MatchEntity> Parse(MatchModel matchModel)
        {
            var matchEntity = new MatchEntity
            {
                MatchDateTime = DateTime.Parse(matchModel.MatchDateTime),
                MatchTitle = matchModel.MatchTitle
            };

            if (await _data.DoesMatchExist(matchEntity.MatchTitle, matchEntity.MatchDateTime))
            {
                throw new Exception(_responses.DUPLICATE_MATCH);
            }

            return matchEntity;
        }

        public async Task<MatchPlayerEntity> Parse(int matchId, int playerID)
        {
            try
            {
                var matchEntity = await _data.GetMatch(matchId);
                if (! await _data.DoesMatchExist(matchId))
                    throw new Exception(_responses.NO_MATCH_EXISTS);

                var playerEntity = await _data.GetPlayer(playerID);
                if (playerEntity == null)
                    throw new Exception(_responses.NO_PLAYER_EXISTS);

                if (await _data.DoesMatchPlayerExist(matchId, playerID))
                {
                    throw new Exception(_responses.DUPLICATE_PLAYER_IN_MATCH);
                }

                return new MatchPlayerEntity { Match = matchEntity, Player = playerEntity };
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Create - Get Model from Entity
        public List<MatchPlayerModel> Create(List<MatchPlayerEntity> matchPlayer)
        {
            return matchPlayer
                .Select(x =>
                    new MatchPlayerModel
                    {
                        MatchPlayerID = x.MatchPlayerID,
                        Player = x.Player
                    }).ToList();
        }
        #endregion
    }
}