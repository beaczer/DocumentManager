namespace LastChance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class commentsidatapublic : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "DateOfPublic", c => c.DateTime());
            AddColumn("dbo.Documents", "Comments", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Documents", "Comments");
            DropColumn("dbo.Documents", "DateOfPublic");
        }
    }
}
