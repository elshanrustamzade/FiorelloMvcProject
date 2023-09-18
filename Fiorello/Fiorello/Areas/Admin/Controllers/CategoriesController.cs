using Fiorello.DAL;
using Fiorello.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fiorello.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly AppDbContext _db;
        public CategoriesController(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            int take = 3;
            ViewBag.PageCount = Math.Ceiling((decimal)(await _db.Categories.CountAsync()) /take);
            List<Category> categories = await _db.Categories.OrderByDescending(x=>x.Id).Skip((page-1)*take).Take(take).ToListAsync();
            return View(categories);
        }
        public IActionResult Create() 
        {
            return View();
        }
    }
}
