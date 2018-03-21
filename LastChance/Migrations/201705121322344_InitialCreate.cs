namespace LastChance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Documents",
                c => new
                    {
                        DocumentId = c.Int(nullable: false, identity: true),
                        DocumentTypeId = c.Int(nullable: false),
                        DocumentFile = c.Binary(nullable: false),
                        Title = c.String(nullable: false, maxLength: 80),
                        Version = c.Int(nullable: false),
                        DateOfAdding = c.DateTime(nullable: false),
                        DateOfAcceptance = c.DateTime(),
                        LineId = c.Int(nullable: false),
                        AcceptedUserId = c.Int(nullable: false),
                        OperationId = c.Int(nullable: false),
                        AuthorUserId = c.Int(nullable: false),
                        StatusId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DocumentId)
                .ForeignKey("dbo.AuthorUsers", t => t.AuthorUserId, cascadeDelete: true)
                .ForeignKey("dbo.AcceptedUsers", t => t.AcceptedUserId, cascadeDelete: true)
                .ForeignKey("dbo.DocumentTypes", t => t.DocumentTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Lines", t => t.LineId, cascadeDelete: true)
                .ForeignKey("dbo.Operations", t => t.OperationId, cascadeDelete: true)
                .ForeignKey("dbo.Status", t => t.StatusId, cascadeDelete: true)
                .Index(t => t.DocumentTypeId)
                .Index(t => t.LineId)
                .Index(t => t.AcceptedUserId)
                .Index(t => t.OperationId)
                .Index(t => t.AuthorUserId)
                .Index(t => t.StatusId);
            
            CreateTable(
                "dbo.AcceptedUsers",
                c => new
                    {
                        AcceptedUserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AcceptedUserId)
                .ForeignKey("dbo.Users", t => t.AcceptedUserId)
                .Index(t => t.AcceptedUserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        Function_EmployeeFunctionId = c.Int(),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.EmployeeFunctions", t => t.Function_EmployeeFunctionId)
                .Index(t => t.Function_EmployeeFunctionId);
            
            CreateTable(
                "dbo.AuthorUsers",
                c => new
                    {
                        AuthorUserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AuthorUserId)
                .ForeignKey("dbo.Users", t => t.AuthorUserId)
                .Index(t => t.AuthorUserId);
            
            CreateTable(
                "dbo.EmployeeFunctions",
                c => new
                    {
                        EmployeeFunctionId = c.Int(nullable: false, identity: true),
                        Function = c.String(),
                    })
                .PrimaryKey(t => t.EmployeeFunctionId);
            
            CreateTable(
                "dbo.DocumentTypes",
                c => new
                    {
                        DocumentTypeId = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.DocumentTypeId);
            
            CreateTable(
                "dbo.Lines",
                c => new
                    {
                        LineId = c.Int(nullable: false, identity: true),
                        LineName = c.String(),
                    })
                .PrimaryKey(t => t.LineId);
            
            CreateTable(
                "dbo.Operations",
                c => new
                    {
                        OperationId = c.Int(nullable: false, identity: true),
                        OperationNumber = c.String(),
                        Description = c.String(),
                        OperationLine_LineId = c.Int(),
                    })
                .PrimaryKey(t => t.OperationId)
                .ForeignKey("dbo.Lines", t => t.OperationLine_LineId)
                .Index(t => t.OperationLine_LineId);
            
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        StatusId = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        NextStatusId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StatusId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Documents", "StatusId", "dbo.Status");
            DropForeignKey("dbo.Documents", "OperationId", "dbo.Operations");
            DropForeignKey("dbo.Documents", "LineId", "dbo.Lines");
            DropForeignKey("dbo.Operations", "OperationLine_LineId", "dbo.Lines");
            DropForeignKey("dbo.Documents", "DocumentTypeId", "dbo.DocumentTypes");
            DropForeignKey("dbo.Documents", "AcceptedUserId", "dbo.AcceptedUsers");
            DropForeignKey("dbo.AcceptedUsers", "AcceptedUserId", "dbo.Users");
            DropForeignKey("dbo.Users", "Function_EmployeeFunctionId", "dbo.EmployeeFunctions");
            DropForeignKey("dbo.AuthorUsers", "AuthorUserId", "dbo.Users");
            DropForeignKey("dbo.Documents", "AuthorUserId", "dbo.AuthorUsers");
            DropIndex("dbo.Operations", new[] { "OperationLine_LineId" });
            DropIndex("dbo.AuthorUsers", new[] { "AuthorUserId" });
            DropIndex("dbo.Users", new[] { "Function_EmployeeFunctionId" });
            DropIndex("dbo.AcceptedUsers", new[] { "AcceptedUserId" });
            DropIndex("dbo.Documents", new[] { "StatusId" });
            DropIndex("dbo.Documents", new[] { "AuthorUserId" });
            DropIndex("dbo.Documents", new[] { "OperationId" });
            DropIndex("dbo.Documents", new[] { "AcceptedUserId" });
            DropIndex("dbo.Documents", new[] { "LineId" });
            DropIndex("dbo.Documents", new[] { "DocumentTypeId" });
            DropTable("dbo.Status");
            DropTable("dbo.Operations");
            DropTable("dbo.Lines");
            DropTable("dbo.DocumentTypes");
            DropTable("dbo.EmployeeFunctions");
            DropTable("dbo.AuthorUsers");
            DropTable("dbo.Users");
            DropTable("dbo.AcceptedUsers");
            DropTable("dbo.Documents");
        }
    }
}
