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
        public int PlayerID { get; set; }

        public string Name { get; set; }

        public int YearOfBirth { get; set; }
    }
}