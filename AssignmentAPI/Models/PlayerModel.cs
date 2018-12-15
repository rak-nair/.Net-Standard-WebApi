using AssignmentAPI.Models.Attribute;
using System.ComponentModel.DataAnnotations;

namespace AssignmentAPI.Models
{
    //Input Model for Player
    public class PlayerModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [RangeUntilCurrentYear(1900)]
        public int YearOfBirth { get; set; }
    }
}