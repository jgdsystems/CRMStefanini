namespace WebSoccer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PasswordRecovey : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "PasswordRecover", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "PasswordRecover");
        }
    }
}
