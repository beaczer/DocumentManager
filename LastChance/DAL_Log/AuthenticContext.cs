using LastChance.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastChance.DAL_Log
{
    public class AutContext : DbContext
    {
        public AutContext() : base("AuthenticContext")
        {

        }
        
    }
}
