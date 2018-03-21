namespace LastChance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dodanieosobyzatwierdzajacejusuniecie : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DeleteUsers",
                c => new
                    {
                        DeletedUserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DeletedUserId)
                .ForeignKey("dbo.Users", t => t.DeletedUserId)
                .Index(t => t.DeletedUserId);
            
            AddColumn("dbo.Documents", "DeleteUser_DeletedUserId", c => c.Int());
            CreateIndex("dbo.Documents", "DeleteUser_DeletedUserId");
            AddForeignKey("dbo.Documents", "DeleteUser_DeletedUserId", "dbo.DeleteUsers", "DeletedUserId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DeleteUsers", "DeletedUserId", "dbo.Users");
            DropForeignKey("dbo.Documents", "DeleteUser_DeletedUserId", "dbo.DeleteUsers");
            DropIndex("dbo.DeleteUsers", new[] { "DeletedUserId" });
            DropIndex("dbo.Documents", new[] { "DeleteUser_DeletedUserId" });
            DropColumn("dbo.Documents", "DeleteUser_DeletedUserId");
            DropTable("dbo.DeleteUsers");
        }
    }
}
