using System.ComponentModel.DataAnnotations;

namespace AssignmentAPI.Data.Entities
{
    //Entity for Players
    public class PlayerEntity
    {
        [Key]
        public int PlayerID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int YearOfBirth { get; set; }
    }
}