using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastChance.Models
{
    public enum EmplCategory { Technolog, Kierownik, Quality, Maintenance}

    public class EmployeeCategory
    {
        [Key]
        public int IdEmployeeCategory { get; set; }
        public EmplCategory Category { get; set; }

    }
}
