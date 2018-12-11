using AssignmentAPI.Models.Attribute;
using System;
using System.ComponentModel.DataAnnotations;

namespace AssignmentAPI.Models
{
    public class PlayerModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [RangeUntilCurrentYear(1900)]
        public int YearOfBirth { get; set; }
    }
}