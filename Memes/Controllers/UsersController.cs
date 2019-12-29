using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MemesPortal.Models;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;
using ReflectionIT.Mvc.Paging;

namespace MemesPortal.Controllers
{
    public class UsersController : Controller
    {
        private readonly MemesDB _context;

        public UsersController(MemesDB context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            if (HttpContext.Session.GetInt32("Login") != 1)
                return RedirectToAction(nameof(Login));
            else
            {
                var linkAggregator = _context.Memes.Include(l => l.Users).Include(l => l.Likes)
                    .Where(u => u.UserId == HttpContext.Session.GetInt32("UserID"))
                    .OrderByDescending(o => o.Likes.Where(l => l.MemesID == o.MemesId).Count());
                return View(await PagingList.CreateAsync(linkAggregator, 100, page));
            }
        }

        public IActionResult Register()
        {
            if (HttpContext.Session.GetInt32("Login") != 1)
                return View();
            else
                return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Register(Users users)
        {
            if (HttpContext.Session.GetInt32("Login") != 1)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(users);
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        var sqlException = ex.InnerException as System.Data.SqlClient.SqlException;

                        if (sqlException.Number == 2601 || sqlException.Number == 2627)
                        {
                            ViewBag.Message = "Już istnieje taki użytkownik!";
                            return View();
                        }
                        else
                        {
                            ViewBag.Message = "Bład podczas dodawania do bazy!";
                            return View();
                        }
                    }
                    ViewBag.Message = users.Email + " Dodany to agregatora!";
                }
                return View();
            }
            else
                return RedirectToAction(nameof(Index));
        }

        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32("Login") != 1)
                return View();
            else
                return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(Users users)
        {
            if (HttpContext.Session.GetInt32("Login") != 1)
            {
                var usr = _context.Users.Where(u => u.Email == users.Email && u.Password == users.Password).FirstOrDefault();
                if (usr != null)
                {
                    HttpContext.Session.SetInt32("UserID", usr.UserId);
                    HttpContext.Session.SetString("UserEmail", usr.Email.ToString());
                    HttpContext.Session.SetInt32("Login", 1);
                    TempData["UserID"] = usr.UserId.ToString();
                    ViewData["UserEmail"] = usr.UserId.ToString();
                    ViewData["Login"] = true;
                    return RedirectToAction("Index", "Home", new { area = "" });
                }
                else
                    ModelState.AddModelError("", "Podane hasło lub Email są nie prawidłowe");
                return View();
            }
            else
                return RedirectToAction(nameof(Index));
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Login");
            HttpContext.Session.Remove("UserID");
            HttpContext.Session.Remove("UserEmail");
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        private bool UsersExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
