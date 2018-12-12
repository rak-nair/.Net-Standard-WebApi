using AssignmentAPI.Models.Attribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AssignmentAPI.Data.Entities
{
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