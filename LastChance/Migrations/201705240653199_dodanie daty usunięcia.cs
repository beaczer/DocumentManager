namespace LastChance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dodaniedatyusunięcia : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "DateOfDelete", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Documents", "DateOfDelete");
        }
    }
}
