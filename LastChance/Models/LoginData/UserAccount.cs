using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastChance.Models.LoginData
{

    public class UserAccount
    {
        [Key]
        public int UserId { get; set; }

        [Column("Imię")]
        [Required(ErrorMessage ="Imię jest wymagane")]
        public string FirstName { get; set; }

        [Column("Nazwisko")]
        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "Email jest wymagany")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3]\.)|(([\w-]+\.)+))([a-zA-Z{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Błędny format adresu email")]
        public string Email { get; set; }

        [Column("Nazwa użytkownika")]
        [Required(ErrorMessage = "Nazwa uzytkownika jest wymagana")]
        public string UserName { get; set; }

        [Column ("Hasło")]
        [Required(ErrorMessage = "Hasło jest wymagane")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Column("Potwierdź hasło")]
        [Compare("Password",ErrorMessage ="Potwierdź hasło")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

    }
}
