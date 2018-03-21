using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LastChance.Models.DocumentData
{
    public class DeleteUser
    {
        public DeleteUser()
        {
            DoccumentDeleted = new List<Document>();
        }

        [Key, ForeignKey("User")]
        public int DeleteUserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Document> DoccumentDeleted { get; set; }
    }
}