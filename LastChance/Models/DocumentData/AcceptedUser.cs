using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LastChance.Models.DocumentData
{
    public class AcceptedUser
    {
        public AcceptedUser()
        {
            DoccumentAccepted = new List<Document>();
        }

        [Key, ForeignKey("User")]
        public int AcceptedUserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Document> DoccumentAccepted { get; set; }
    }
}