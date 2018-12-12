using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AssignmentAPI.Data.Entities
{
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