using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MemesPortal.Models;
using Microsoft.AspNetCore.Http;

namespace MemesPortal.Controllers
{
    public class LikesController : Controller
    {
        private readonly MemesDB _context;

        public LikesController(MemesDB context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Like([Bind("LikeId,UserID,LinkID")] Likes likes, int id)
        {
            var model = _context.Likes.Where(u => u.UserID == HttpContext.Session.GetInt32("UserID") && u.MemesID == id).Any();
            if (model == false)
            {
                likes.MemesID = id;
                likes.UserID = (int)HttpContext.Session.GetInt32("UserID");
                if (ModelState.IsValid)
                {
                    _context.Add(likes);
                    await _context.SaveChangesAsync();
                    return NoContent();
                }
                ViewData["LinkID"] = new SelectList(_context.Memes, "LinkId", "Link", likes.MemesID);
                ViewData["UserID"] = new SelectList(_context.Users, "UserId", "Email", likes.UserID);
                return NoContent();
            }
            else
            {
                _context.Likes.Remove(await _context.Likes.Where(u => u.UserID == HttpContext.Session.GetInt32("UserID") && u.MemesID == id).FirstAsync());
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }

        private bool LikesExists(int id)
        {
            return _context.Likes.Any(e => e.LikeId == id);
        }
    }
}
