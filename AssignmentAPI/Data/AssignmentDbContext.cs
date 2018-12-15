using AssignmentAPI.Data.Entities;
using System.Data.Entity;

namespace AssignmentAPI.Data
{
    //DBContext for the application
    public class AssignmentDbContext: DbContext
    {
        public AssignmentDbContext(): base("DefaultConnection")
        {
           
        }

        public DbSet<MatchEntity> Matches { get; set; }

        public DbSet<PlayerEntity> Players { get; set; }

        public DbSet<MatchPlayerEntity> MatchPlayers { get; set; }
        
    }
}