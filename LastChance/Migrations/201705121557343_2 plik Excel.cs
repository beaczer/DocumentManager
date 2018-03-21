namespace LastChance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2plikExcel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "DocumentFileExcel", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Documents", "DocumentFileExcel");
        }
    }
}
