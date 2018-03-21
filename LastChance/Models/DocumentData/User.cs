using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace LastChance.Models.DocumentData
{
    

    public class User
    {
        public User()
        {
        }
        [Key]
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public virtual EmployeeFunction Function { get; set; }

        public virtual AuthorUser AuthorUser { get; set; }
        public virtual AcceptedUser AcceptUser { get; set; }
        public virtual DeleteUser DeleteUser { get; set; }
    }
}
