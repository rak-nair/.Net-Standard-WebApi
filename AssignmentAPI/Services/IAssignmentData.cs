using AssignmentAPI.Data.Entities;
using System.Collections.Generic;

namespace AssignmentAPI.Services
{
    public interface IAssignmentData
    {
        IEnumerable<PlayerEntity> GetAllPlayers();

        PlayerEntity GetPlayer(int playerId);

        PlayerEntity AddPlayer(PlayerEntity player);

        MatchEntity GetMatch(int matchId);

        IEnumerable<MatchEntity> GetAllMatches();

        MatchEntity AddMatch(MatchEntity match);

        IEnumerable<MatchPlayerEntity> GetMatchPlayersInMatch(int matchId);

        MatchPlayerEntity AddPlayerToMatch(MatchPlayerEntity matchPlayer);
    }
}