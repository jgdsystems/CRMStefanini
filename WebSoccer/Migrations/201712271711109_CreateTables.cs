namespace WebSoccer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Logs",
                c => new
                    {
                        Id_log = c.Int(nullable: false, identity: true),
                        Datetime = c.DateTime(nullable: false),
                        Description = c.String(),
                        Id_user = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id_log);
            
            CreateTable(
                "dbo.Match",
                c => new
                    {
                        Id_match = c.Int(nullable: false, identity: true),
                        Team_id_team_A = c.Long(nullable: false),
                        Team_id_team_B = c.Long(nullable: false),
                        Score_a = c.Int(nullable: false),
                        Score_b = c.Int(nullable: false),
                        Winner = c.Long(nullable: false),
                        Address = c.String(),
                        Date = c.DateTime(nullable: false),
                        Confirmation = c.DateTime(nullable: false),
                        Status = c.String(),
                        Latitude = c.String(),
                        Longitude = c.String(),
                        Users_id_users = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id_match)
                .ForeignKey("dbo.Team", t => t.Team_id_team_A, cascadeDelete: false)
                .ForeignKey("dbo.Team", t => t.Team_id_team_B, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.Users_id_users, cascadeDelete: false)
                .Index(t => t.Team_id_team_A)
                .Index(t => t.Team_id_team_B)
                .Index(t => t.Users_id_users);
            
            CreateTable(
                "dbo.Team",
                c => new
                    {
                        Id_team = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Users_id_users = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id_team)
                .ForeignKey("dbo.Users", t => t.Users_id_users, cascadeDelete: false)
                .Index(t => t.Users_id_users);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id_users = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false, maxLength: 100),
                        Password = c.String(nullable: false, maxLength: 100),
                        Type = c.String(maxLength: 45),
                        Create_date = c.DateTime(nullable: false),
                        Last_access = c.DateTime(nullable: false),
                        Active = c.Int(nullable: false),
                        Profile = c.String(maxLength: 60),
                    })
                .PrimaryKey(t => t.Id_users);
            
            CreateTable(
                "dbo.Players",
                c => new
                    {
                        Id_player = c.Long(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        Phonenumber = c.String(maxLength: 45),
                        Email = c.String(maxLength: 100),
                        Type = c.String(maxLength: 45),
                        Status = c.String(maxLength: 60),
                        Users_id_users = c.Int(nullable: false),
                        Id_Positions = c.Int(nullable: false),
                        Active = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id_player)
                .ForeignKey("dbo.Positions", t => t.Id_Positions, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.Users_id_users, cascadeDelete: false)
                .Index(t => t.Users_id_users)
                .Index(t => t.Id_Positions);
            
            CreateTable(
                "dbo.Positions",
                c => new
                    {
                        Id_Positions = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Active = c.Int(nullable: false),
                        X = c.Int(nullable: false),
                        Y = c.Int(nullable: false),
                        Id_Users = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Positions)
                .ForeignKey("dbo.Users", t => t.Id_Users, cascadeDelete: false)
                .Index(t => t.Id_Users);
            
            CreateTable(
                "dbo.Selection",
                c => new
                    {
                        Id_selection = c.Long(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Active = c.Int(nullable: false),
                        Team_id_team = c.Long(nullable: false),
                        Players_id_player = c.Long(),
                        Users_id_users = c.Int(nullable: false),
                        Positions_id_positions = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id_selection)
                .ForeignKey("dbo.Players", t => t.Players_id_player)
                .ForeignKey("dbo.Positions", t => t.Positions_id_positions, cascadeDelete: false)
                .ForeignKey("dbo.Team", t => t.Team_id_team, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.Users_id_users, cascadeDelete: false)
                .Index(t => t.Team_id_team)
                .Index(t => t.Players_id_player)
                .Index(t => t.Users_id_users)
                .Index(t => t.Positions_id_positions);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Selection", "Users_id_users", "dbo.Users");
            DropForeignKey("dbo.Selection", "Team_id_team", "dbo.Team");
            DropForeignKey("dbo.Selection", "Positions_id_positions", "dbo.Positions");
            DropForeignKey("dbo.Selection", "Players_id_player", "dbo.Players");
            DropForeignKey("dbo.Players", "Users_id_users", "dbo.Users");
            DropForeignKey("dbo.Players", "Id_Positions", "dbo.Positions");
            DropForeignKey("dbo.Positions", "Id_Users", "dbo.Users");
            DropForeignKey("dbo.Match", "Users_id_users", "dbo.Users");
            DropForeignKey("dbo.Match", "Team_id_team_B", "dbo.Team");
            DropForeignKey("dbo.Match", "Team_id_team_A", "dbo.Team");
            DropForeignKey("dbo.Team", "Users_id_users", "dbo.Users");
            DropIndex("dbo.Selection", new[] { "Positions_id_positions" });
            DropIndex("dbo.Selection", new[] { "Users_id_users" });
            DropIndex("dbo.Selection", new[] { "Players_id_player" });
            DropIndex("dbo.Selection", new[] { "Team_id_team" });
            DropIndex("dbo.Positions", new[] { "Id_Users" });
            DropIndex("dbo.Players", new[] { "Id_Positions" });
            DropIndex("dbo.Players", new[] { "Users_id_users" });
            DropIndex("dbo.Team", new[] { "Users_id_users" });
            DropIndex("dbo.Match", new[] { "Users_id_users" });
            DropIndex("dbo.Match", new[] { "Team_id_team_B" });
            DropIndex("dbo.Match", new[] { "Team_id_team_A" });
            DropTable("dbo.Selection");
            DropTable("dbo.Positions");
            DropTable("dbo.Players");
            DropTable("dbo.Users");
            DropTable("dbo.Team");
            DropTable("dbo.Match");
            DropTable("dbo.Logs");
        }
    }
}
