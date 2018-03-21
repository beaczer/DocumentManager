using LastChance.Models.DocumentData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastChance.Models.ViewModels
{
    public class UserBoxVM
    {
        [Required]
        [Display(Name = "Index")]
        public string Index { get; set; }
        [Required]
        [Display(Name = "Id Dokumentu")]
        public int DocumentId { get; set; }
        [Required]
        [Display(Name = "Plik do weryfikacji")]
        public byte[] FileToCheck { get; set; }
        [Required]
        [Display(Name = "Data akceptacji")]
        public DateTime? DateOfAccept { get; set; }
        [Required]
        [Display(Name = "Status dokumentu")]
        public string Status { get; set; }
        [Required]
        [Display(Name = "Komentarz")]
        public string Comments { get; set; }
        



    }
}
