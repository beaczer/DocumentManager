using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastChance.Models
{
    [Table("Document")]
    public class Document
    {
        [Key]
        public int DocumentId { get; set; }
        [ForeignKey("AuthorUser")]
        public int IdAuthorUser  {get;set;}
        public int IdAcceptUser{ get; set; }
        public string Description { get; set; }
        public DocumentVariant DocumentVar { get; set; }
        public int Wersja { get; set; }
        public DateTime DateOfAdding { get; set; }
        public DateTime DateOfAcceptance { get; set; }
        public byte[] DocumentFile { get; set; }

        public virtual Line Line { get; set; }
        public virtual AcceptUser AcceptUser { get; set; }
        public virtual AuthorUser AuthorUser { get; set; }
        public virtual ICollection<AcceptUser> AcceptUsers { get; set; }

    }
}
