using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LastChance.Models.DocumentData
{
    public class AuthorUser
    {
        public AuthorUser()
        {
            DocumentCreated = new List<Document>();
        }

        [Key, ForeignKey("User")]
        public int AuthorUserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Document> DocumentCreated { get; set; }

    }
}