using LastChance.Models.DocumentData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastChance.Models.ViewModels
{
    public class DocumentAndTypeVM
    {
        [Required]
        [Display(Name = "Index")]
        public string Index { get; set; }
        [Required]
        [Display(Name="Id dokumentu")]
        public int DocumentId { get; set; }
        [Required]
        [Display(Name ="Tytuł")]
        public string Title { get; set; }
        [Required]
        [Display(Name ="Typ dokumentu")]
        public string DocumentTypeDescription { get; set; }
        public int LineId { get; set; }
        [Required]
        [Display(Name="Linia")]
        public string LineName { get; set; }
        [Required]
        [Display(Name="Numer operacji")]
        public string OperationNumber { get; set; }
        [Required]
        [Display(Name ="Opis Operacji")]
        public string DescriptionOp { get; set; }
        [Required]
        [Display(Name="Wersja")]
        public int Version { get; set; }
        [Required]
        [Display(Name ="Plik Pdf")]
        public byte[] DocumentFile { get; set; }
        [Required]
        [Display(Name ="Plik Excel")]
        public byte[] DocumentFileExcel { get; set; }
        

    }
}
