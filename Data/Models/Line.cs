using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastChance.Models
{
  public class Line
    {
        [Key]
        public int IdLine { get; set; }
        public string LineName { get; set; }
        

    }
}
