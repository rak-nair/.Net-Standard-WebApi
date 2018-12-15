using System.ComponentModel.DataAnnotations;

namespace AssignmentAPI.Data.Entities
{
    //Entity for Players in a Match
    public class MatchPlayerEntity
    {
        [Key]
        public int MatchPlayerID { get; set; }
        [Required]
        public MatchEntity Match { get; set; }
        [Required]
        public PlayerEntity Player { get; set; }
    }
}