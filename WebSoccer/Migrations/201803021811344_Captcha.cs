namespace WebSoccer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Captcha : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Captcha", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Captcha");
        }
    }
}
