﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastChance.Models
{
    [Table("AcceptUser")]
   public class AcceptUser
    {
        [Key]
        public int IdAcceptUser { get; set; }
        public int IdUser { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
    }
}
