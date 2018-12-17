using AssignmentAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AssignmentAPI.Services
{
    //Base interface for all Data operations.
    public interface IAssignmentData
    {
        #region Player_Data_Methods
        //Using a tuple since we already have input and output Models for this data, 
        //and the Tuple is only used for intermediate data transformation.
        Task<(List<PlayerEntity> Players, int TotalRows)> GetAllPlayers(int page, int pageSize);

        Task<PlayerEntity> GetPlayer(int playerId);

        Task<bool> DoesPlayerExist(string name, int yearOfBirth);

        Task<PlayerEntity> AddPlayer(PlayerEntity player);
        #endregion

        #region Match_Data_Methods
        //Using a tuple since we already have input and output Models for this data, 
        //and the Tuple is only used for intermediate data transformation.
        Task<(List<MatchEntity> Matches, int TotalRows)> GetAllMatches(int page, int pageSize);

        Task<MatchEntity> GetMatch(int matchId);

        Task<bool> DoesMatchExist(string matchTitle, DateTime matchDateTime);

        Task<bool> DoesMatchExist(int matchID);

        Task<MatchEntity> AddMatchAsync(MatchEntity match);
        #endregion

        #region MatchPlayer_Data_Methods
        //Using a tuple since we already have input and output Models for this data, 
        //and the Tuple is only used for intermediate data transformation.
        Task<(List<MatchPlayerEntity> MatchPlayers, int TotalRows)> GetMatchPlayersInMatch
                    (int matchId, int page, int pageSize);

        Task<bool> DoesMatchPlayerExist(int matchID, int playerID);

        Task<MatchPlayerEntity> AddPlayerToMatch(MatchPlayerEntity matchPlayer);
        #endregion
    }
}