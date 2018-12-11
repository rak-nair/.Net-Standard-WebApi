using AssignmentAPI.Data.Entities;

namespace AssignmentAPI.Models
{
    public class MatchPlayerModel
    {
        public int MatchPlayerID { get; set; }
        public PlayerEntity Player { get; set; }
    }
}