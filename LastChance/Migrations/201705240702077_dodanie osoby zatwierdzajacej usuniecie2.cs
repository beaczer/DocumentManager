namespace LastChance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dodanieosobyzatwierdzajacejusuniecie2 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.DeleteUsers", name: "DeletedUserId", newName: "DeleteUserId");
            RenameColumn(table: "dbo.Documents", name: "DeleteUser_DeletedUserId", newName: "DeleteUserId");
            RenameIndex(table: "dbo.DeleteUsers", name: "IX_DeletedUserId", newName: "IX_DeleteUserId");
            RenameIndex(table: "dbo.Documents", name: "IX_DeleteUser_DeletedUserId", newName: "IX_DeleteUserId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Documents", name: "IX_DeleteUserId", newName: "IX_DeleteUser_DeletedUserId");
            RenameIndex(table: "dbo.DeleteUsers", name: "IX_DeleteUserId", newName: "IX_DeletedUserId");
            RenameColumn(table: "dbo.Documents", name: "DeleteUserId", newName: "DeleteUser_DeletedUserId");
            RenameColumn(table: "dbo.DeleteUsers", name: "DeleteUserId", newName: "DeletedUserId");
        }
    }
}
