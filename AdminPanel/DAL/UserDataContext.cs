using AdminPanel.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.DAL
{
    class UserDataContext : DbContext
    {
        public UserDataContext() :base("UserDataContext")
        {
        }
        public DbSet<UserData> userData { get; set; }
    }
}
