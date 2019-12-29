using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MemesPortal.Models
{
    public class Memes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MemesId { get; set; }

        [Required(ErrorMessage = "Należy podać nazwe")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Należy podać jakiś link")]
        [Url(ErrorMessage = "Proszę podać prawidłowy link")]
        public string Link { get; set; }

        public DateTime Date { get; set; }

        //public int TotalLikes { get; set; }

        [ForeignKey("Users")]
        public int UserId { get; set; }

        public virtual Users Users { get; set; }

        public virtual ICollection<Likes> Likes { get; set; }
    }
}
