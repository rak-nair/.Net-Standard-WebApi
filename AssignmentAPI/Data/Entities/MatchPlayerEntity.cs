using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AssignmentAPI.Data.Entities
{
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