using Fiorello.DAL;
using Fiorello.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fiorello.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _db;
        public ProductsController(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.ProductsCount = await _db.Products.CountAsync(x => !x.IsDeactive);

            List<Product> products = await _db.Products.Where(x => !x.IsDeactive).OrderByDescending(x => x.Id).Take(8).ToListAsync();
            return View(products);
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
                return NotFound();
            Product product = await _db.Products.Include(x => x.ProductDetail).FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
                return BadRequest();
            return View(product);
        }

        public async Task<IActionResult> LoadMore(int skip)
        {
            int productsCount = await _db.Products.Where(x => !x.IsDeactive).CountAsync();

            if (productsCount <= skip)
            {
                return Content("Get Out!");
            }

            List<Product> products = await _db.Products.OrderByDescending(x => x.Id).Skip(skip).Take(8).ToListAsync();

            return PartialView("_LoadMoreProductsPartial", products);
        }


    }
}
