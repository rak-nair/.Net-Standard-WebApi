using System.ComponentModel.DataAnnotations;

namespace AssignmentAPI.Models
{
    //Input model for Matches
    public class MatchModel
    {
        [Required(AllowEmptyStrings = false)]
        [MinLength(8)]
        public string MatchDateTime { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MinLength(5)]
        public string MatchTitle { get; set; }
    }
}