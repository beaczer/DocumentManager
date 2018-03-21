using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastChance.Models.DocumentData
{
    public class Document
    {
        [Key]
        [ScaffoldColumn(false)]
        public int DocumentId { get; set; }

        
        public int DocumentTypeId { get; set; }

        [Display(Name = "Typ dokumentu")]
        public DocumentType DocumentType { get; set; }

        [Display(Name = "Plik dokumentu")]
        public byte[] DocumentFile { get; set; }

        public string Index { get; set;}

        [ScaffoldColumn(false)]
        public byte[] DocumentFileExcel { get; set; }


        [Display(Name="Opis instrukcji")]
        [Required(ErrorMessage ="Musisz wprowadzić opis")]
        [StringLength(80)]
        public string Title { get; set; }

        [Display(Name = "Wersja")]
        [ScaffoldColumn(false)]
        public int Version { get; set; }

        [ScaffoldColumn(false)]
        public DateTime DateOfAdding { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? DateOfAcceptance { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? DateOfPublic { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? DateOfDelete { get; set; }

        public int LineId { get; set; }
        [Display(Name = "Wybierz linię")]
        public virtual Line Line { get; set; }

        public int AcceptedUserId { get; set; }
        [Display(Name = "Wybierz osobę akceptującą")]
        public virtual AcceptedUser AccepedUser { get; set; }

        [ScaffoldColumn(false)]
        public int? DeleteUserId { get; set; }
        public virtual DeleteUser DeleteUser { get; set; }


        public int OperationId { get; set; }
        [Display(Name = "Wybierz operację")]
        public virtual Operation Operation { get; set; }

        [ScaffoldColumn(false)]
        public int AuthorUserId { get; set; }
        [ScaffoldColumn(false)]
        public virtual AuthorUser AuthorUser { get; set; }

        [ScaffoldColumn(false)]
        public int StatusId { get; set; }
        public virtual Status Status { get; set; }

        [ScaffoldColumn(false)]
        public string Comments { set; get; }
    }
}
