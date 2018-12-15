using AssignmentAPI.Data.Entities;

namespace AssignmentAPI.Models
{
    //Output Model for Players in a Match
    public class MatchPlayerModel
    {
        public int MatchPlayerID { get; set; }
        public PlayerEntity Player { get; set; }
    }
}