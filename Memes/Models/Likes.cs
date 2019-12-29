using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MemesPortal.Models
{
    public class Likes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LikeId { get; set; }

        [ForeignKey("Users")]
        public int UserID { get; set; }

        [ForeignKey("Memes")]
        public int MemesID { get; set; }

        public virtual Memes Memes { get; set; }

        public virtual Users Users { get; set; }
    }
}
