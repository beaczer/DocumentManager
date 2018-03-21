using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastChance.Models.DocumentData
{
  
    public class Status
    {
        public Status()
        {
            Documents = new List<Document>();
        }
        [Key]
        public int StatusId { get; set; }
        public string Description { get; set; }
        public int NextStatusId { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<Document> Documents { get; set; }
    }
}
