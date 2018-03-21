using LastChance.Models.DocumentData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastChance.Models.ViewModels
{
    public class DocumentUserVM
    {
        public Document document { get; set; }
        public User user { get; set; }
    }
}
