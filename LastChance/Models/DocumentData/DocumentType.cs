using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastChance.Models.DocumentData
{
  
    public class DocumentType
    {
        public DocumentType()
        {
            DocCollection = new List<Document>();
        }

        public int DocumentTypeId { get; set; }

        [Display(Name = "Typ dokumentu")]
        public string Description { get; set; }

        public virtual ICollection<Document> DocCollection { get; set; }

    }
}
