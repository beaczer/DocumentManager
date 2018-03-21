namespace Data
{
    using LastChance.Models;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class DocumentContext : DbContext
    {
        // Your context has been configured to use a 'DocumentContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'Data.DocumentContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'DocumentContext' 
        // connection string in the application configuration file.
        public DocumentContext()
            : base("name=DocumentContext")
            {

            }
            public DbSet<Document> Documents { get; set; }
            public DbSet<AcceptUser> AcceptUsers { get; set; }
            public DbSet<AuthorUser> AuthorUsers { get; set; }
            public DbSet<Line> Lines { get; set; }
            public DbSet<Operation> Operations { get; set; }
            public DbSet<EmployeeCategory> EmployessCategories { get; set; }
            public DbSet<DocumentVariant> DocumentVariants { get; set; }
            public DbSet<DocumentStatus> DocumentsStatus { get; set; }
        }
    }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
