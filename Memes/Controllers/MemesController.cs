using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MemesPortal.Models;
using Microsoft.AspNetCore.Http;
using ReflectionIT.Mvc.Paging;
using System.IO;

namespace MemesPortal.Controllers
{
    public class MemesController : Controller
    {
        private readonly MemesDB _context;

        public MemesController(MemesDB context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MemesId,Name,Image,Date,UserId")] Memes memes, IFormFile Image)
        {
            if (ModelState.IsValid)
            {
                using (var ms = new MemoryStream())
                {
                    Image.CopyTo(ms);
                    memes.Image = ms.ToArray();
                }

                memes.Date = DateTime.Now;
                memes.UserId = (int)HttpContext.Session.GetInt32("UserID");
                _context.Add(memes);
                await _context.SaveChangesAsync();
                ViewBag.MessageMemesAdd = "Link dodany to agregatora!";
                return View();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", memes.UserId);
            return View();
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var memes = await _context.Memes.FindAsync(id);
            if (memes == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", memes.UserId);
            return View(memes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MemesId,Name,Link,Date,UserId")] Memes memes)
        {
            if (id != memes.MemesId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(memes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemesExists(memes.MemesId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Users", new { area = "" });
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", memes.UserId);
            return View(memes);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var memes = await _context.Memes
                .Include(l => l.Users)
                .FirstOrDefaultAsync(m => m.MemesId == id);
            if (memes == null)
            {
                return NotFound();
            }

            return View(memes);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var memes = await _context.Memes.FindAsync(id);
            _context.Memes.Remove(memes);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Users", new { area = "" });
        }

        private bool MemesExists(int id)
        {
            return _context.Memes.Any(e => e.MemesId == id);
        }
    }
}
