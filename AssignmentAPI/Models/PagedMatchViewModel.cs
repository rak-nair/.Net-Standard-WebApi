using AssignmentAPI.Data.Entities;
using AssignmentAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssignmentAPI.Models
{
    public class PagedMatchViewModel
    {
        public List<MatchEntity> Matches { get; set; }
        public PagingLinkBuilder Pages { get; set; }
    }
}