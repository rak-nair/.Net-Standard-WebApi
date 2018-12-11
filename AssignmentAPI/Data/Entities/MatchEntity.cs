using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssignmentAPI.Data.Entities
{
    public class MatchEntity
    {
        public MatchEntity()
        {
            //Players = new List<PlayerEntity>();
        }

        public int MatchID { get; set; }
        public DateTime MatchDateTime { get; set; }
        public string MatchTitle { get; set; }
        //public List<PlayerEntity> Players { get; set; }
    }
}