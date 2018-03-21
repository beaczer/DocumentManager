using LastChance.Models.DocumentData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastChance.Models.ViewModels
{
    public class DocumentUserDelVM
    {
        public DocumentUserDelVM(int DocId, int UserId, string fName, string lName, string email, int FunctionId)
        {
            this.DocId = DocId;
            this.UserId = UserId;
            this.FunctionId = FunctionId;
            this.FirstName = fName;
            this.LastName = lName;
            this.Email = email;
        }
        public int DocId { get; set; }
        public int UserId { get; set; }
        public int FunctionId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        
    }
}
