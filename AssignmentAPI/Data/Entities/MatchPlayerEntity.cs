using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssignmentAPI.Data.Entities
{
    public class MatchPlayerEntity
    {
        public int MatchPlayerID { get; set; }

        public MatchEntity Match { get; set; }

        public PlayerEntity Player { get; set; }
    }
}