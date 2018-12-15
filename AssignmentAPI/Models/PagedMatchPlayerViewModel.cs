using AssignmentAPI.Services;
using System.Collections.Generic;

namespace AssignmentAPI.Models
{
    //Output Model for Paged Players in a Match
    public class PagedMatchPlayerViewModel
    {
        public List<MatchPlayerModel> MatchPlayers { get; set; }
        public PagingLinkBuilder Pages { get; set; }
    }
}