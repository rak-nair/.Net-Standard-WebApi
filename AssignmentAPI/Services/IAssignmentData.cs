using AssignmentAPI.Data.Entities;
using System.Collections.Generic;

namespace AssignmentAPI.Services
{
    public interface IAssignmentData
    {
        IEnumerable<PlayerEntity> GetAllPlayers();

        PlayerEntity GetPlayer(int playerId);

        bool AddPlayer(PlayerEntity player);

        MatchEntity GetMatch(int matchId);

        IEnumerable<MatchEntity> GetAllMatches();

        bool AddMatch(MatchEntity match);

        IEnumerable<MatchPlayerEntity> GetPlayersInMatch(int matchId);

        bool AddPlayerToMatch(MatchPlayerEntity matchPlayer);
    }
}