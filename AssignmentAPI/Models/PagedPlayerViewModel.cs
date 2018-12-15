using AssignmentAPI.Data.Entities;
using AssignmentAPI.Services;
using System.Collections.Generic;

namespace AssignmentAPI.Models
{
    //Output Model for Paged Players
    public class PagedPlayerViewModel
    {
        public List<PlayerEntity> Players { get; set; }
        public PagingLinkBuilder Pages { get; set; }
    }
}