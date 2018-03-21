using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastChance.Models
{
   public class Operation
    {
        [Key]
        public int IdOperation { get; set; }
        public int OperationNumber { get; set; }
        public int IdLine { get; set; }
        public int MachineNumber { get; set; }


    }
}
