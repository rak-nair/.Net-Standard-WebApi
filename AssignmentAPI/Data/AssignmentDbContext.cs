using AssignmentAPI.Data.Entities;
using System.Data.Entity;
using System.Diagnostics;

namespace AssignmentAPI.Data
{
    public class AssignmentDbContext: DbContext
    {
        public AssignmentDbContext(): base("DefaultConnection")
        {
           
        }

        public DbSet<MatchEntity> Matches { get; set; }

        public DbSet<PlayerEntity> Players { get; set; }

        public DbSet<MatchPlayerEntity> MatchPlayers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.Log = (x) => Debug.Write(x);
        }
    }
}