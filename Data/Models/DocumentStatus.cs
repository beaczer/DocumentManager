using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastChance.Models
{
    public enum documentStatus { OczekujeNaZatwierdzenie, Zatwierdzona, Wydana, Usunieta }

    public class DocumentStatus
    {
        [Key]
        public int IdStatus { get; set; }
        public documentStatus DocStatus { get; set; }

        
    }
}
