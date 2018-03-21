using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastChance.Models.DocumentData
{
    public class DocumentDbContext : DbContext
    {
        
        public DocumentDbContext() : base()
        {

        }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<Line> Lines { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<EmployeeFunction> EmplFunctions { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<AcceptedUser> UserAccept { get; set; }
        public DbSet<AuthorUser> UserAuthor { get; set; }
        public DbSet<DeleteUser> DeleteAuthor { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
