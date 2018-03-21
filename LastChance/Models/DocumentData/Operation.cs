using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastChance.Models.DocumentData
{
    public class Operation
    {
        public Operation()
        {
            Documents = new List<Document>();
        }
        [Key]
        public int OperationId { get; set; }

        [Display(Name = "Numer operacji")]
        public string OperationNumber { get; set; }

        public string Description { get; set; }

        
        //public int LineId { get; set; }
        public virtual Line OperationLine { get; set; }

        public virtual ICollection<Document>Documents{ get; set; }
    }
}
