using System.Collections.Generic;

namespace LastChance.Models.DocumentData
{
    public class EmployeeFunction
    {

        public EmployeeFunction()
        {
            Users = new List<User>();
        }

        public int EmployeeFunctionId { get; set; }
        public string Function { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}