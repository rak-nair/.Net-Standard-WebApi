namespace AssignmentAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MatchEntities",
                c => new
                    {
                        MatchID = c.Int(nullable: false, identity: true),
                        MatchDateTime = c.DateTime(nullable: false),
                        MatchTitle = c.String(),
                    })
                .PrimaryKey(t => t.MatchID);
            
            CreateTable(
                "dbo.MatchPlayerEntities",
                c => new
                    {
                        MatchPlayerID = c.Int(nullable: false, identity: true),
                        Match_MatchID = c.Int(),
                        Player_PlayerID = c.Int(),
                    })
                .PrimaryKey(t => t.MatchPlayerID)
                .ForeignKey("dbo.MatchEntities", t => t.Match_MatchID)
                .ForeignKey("dbo.PlayerEntities", t => t.Player_PlayerID)
                .Index(t => t.Match_MatchID)
                .Index(t => t.Player_PlayerID);
            
            CreateTable(
                "dbo.PlayerEntities",
                c => new
                    {
                        PlayerID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        YearOfBirth = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PlayerID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MatchPlayerEntities", "Player_PlayerID", "dbo.PlayerEntities");
            DropForeignKey("dbo.MatchPlayerEntities", "Match_MatchID", "dbo.MatchEntities");
            DropIndex("dbo.MatchPlayerEntities", new[] { "Player_PlayerID" });
            DropIndex("dbo.MatchPlayerEntities", new[] { "Match_MatchID" });
            DropTable("dbo.PlayerEntities");
            DropTable("dbo.MatchPlayerEntities");
            DropTable("dbo.MatchEntities");
        }
    }
}
