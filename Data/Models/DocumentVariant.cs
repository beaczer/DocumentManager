using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastChance.Models
{
    public enum documentVariant { InstrukjaPrzezbrojenia, InstrukcjaOperacyjna, InstrukcjaBezpieczeństwa }

    public class DocumentVariant
    {
        [Key]
        public int IdDocumentVariant { get; set; }
        public documentVariant VariantDecription { get; set; }


    }
}
