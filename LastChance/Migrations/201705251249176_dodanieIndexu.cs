namespace LastChance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dodanieIndexu : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "Index", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Documents", "Index");
        }
    }
}
