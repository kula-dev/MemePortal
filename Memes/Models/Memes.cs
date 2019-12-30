using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
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

        [Display(Name = "Obrazek")]
        [Required(ErrorMessage = "Wymagany Obrazek.")]
        [DataType(DataType.Upload)]
        [FileExtensions(Extensions = "jpg,jpeg")]
        public byte[] Image { get; set; }

        public DateTime Date { get; set; }

        [ForeignKey("Users")]
        public int UserId { get; set; }

        public virtual Users Users { get; set; }

        public virtual ICollection<Likes> Likes { get; set; }
    }
}
