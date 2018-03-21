namespace LastChance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class plik2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Documents", "DocumentFile", c => c.Binary());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Documents", "DocumentFile", c => c.Binary(nullable: false));
        }
    }
}
