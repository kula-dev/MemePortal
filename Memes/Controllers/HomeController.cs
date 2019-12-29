using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MemesPortal.Models;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;

namespace MemesPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly MemesDB _context;

        public HomeController(MemesDB context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var linkAggregator = _context.Memes.Include(l => l.Users).Include(l => l.Likes)
                .Where(d => d.Date >= DateTime.Now.AddDays(-5))
                .OrderByDescending(o => o.Likes.Where(l => l.MemesID == o.MemesId).Count());
                
            return View(await PagingList.CreateAsync(linkAggregator, 100, page));
        }

        public IActionResult Privacy()
        {
            ViewData["Message"] = "Polityka prywatnośći";

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
