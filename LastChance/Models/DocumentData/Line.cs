using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastChance.Models.DocumentData
{
  public class Line
    {
        public Line()
        {
            Documents = new HashSet<Document>();
            Operations = new HashSet<Operation>();
        }
        [Key]
        public int LineId { get; set; }
        [Display(Name = "Nazwa linii")]
        public string LineName { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<Operation> Operations { get; set; }
    }
}
