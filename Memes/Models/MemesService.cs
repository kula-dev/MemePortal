using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MemesPortal.Models
{
    public class MemesService
    {
        public Memes Link { get; set; }
        public IEnumerable<Memes> Links { get; set; }
    }
}
