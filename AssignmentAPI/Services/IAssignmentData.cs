using AssignmentAPI.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssignmentAPI.Services
{
    public interface IAssignmentData
    {
        IQueryable<PlayerEntity> GetAllPlayers();

        Task<PlayerEntity> GetPlayer(int playerId);

        Task<PlayerEntity> AddPlayer(PlayerEntity player);

        IQueryable<MatchEntity> GetAllMatches();

        Task<MatchEntity> GetMatch(int matchId);

        Task<MatchEntity> AddMatchAsync(MatchEntity match);

        IQueryable<MatchPlayerEntity> GetMatchPlayersInMatch(int matchId);

        Task<MatchPlayerEntity> AddPlayerToMatch(MatchPlayerEntity matchPlayer);
    }
}