using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MemesPortal.Models
{
    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Należy podać adres Email!")]
        [EmailAddress(ErrorMessage = "Nie prawidłowy adres Email!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane!")]
        [DataType("Password")]
        public string Password { get; set; }

        [NotMapped]
        [DataType("Password")]
        [Compare("Password", ErrorMessage = "Hasła nie są takie same!")]
        public string ConfirmPassword { get; set; }

        public virtual ICollection<Memes> Links { get; set; }

        public virtual ICollection<Likes> Likes { get; set; }
    }
}
