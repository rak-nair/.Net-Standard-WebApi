using System;
using System.ComponentModel.DataAnnotations;

namespace AssignmentAPI.Data.Entities
{
    //Entity for Matches
    public class MatchEntity
    {
        [Key]
        public int MatchID { get; set; }
        [Required]
        public DateTime MatchDateTime { get; set; }
        [Required]
        public string MatchTitle { get; set; }
    }
}