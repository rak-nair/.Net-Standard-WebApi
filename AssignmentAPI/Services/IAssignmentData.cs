using AssignmentAPI.Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace AssignmentAPI.Services
{
    //Base interface for all Data operations.
    //IQueryable is used for better performance as compared to IEnumerable.
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

        ErrorResponses ErrorResposnses { get; set; }
    }
}