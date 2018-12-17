using AssignmentAPI.Data.Entities;
using System.Data.Entity;

namespace AssignmentAPI.Data
{
    //DBContext for the application
    public class AssignmentDbContext: DbContext
    {
        #region Constructor
        public AssignmentDbContext(): base("DefaultConnection")
        {
           
        }
        #endregion

        #region DBSets
        public DbSet<MatchEntity> Matches { get; set; }

        public DbSet<PlayerEntity> Players { get; set; }

        public DbSet<MatchPlayerEntity> MatchPlayers { get; set; }
        #endregion
    }
}