using Fiorello.DAL;
using Fiorello.Helpers;
using Fiorello.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Fiorello.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public ProductsController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Product> products = await _db.Products.Include(x => x.Category).ToListAsync();
            return View(products);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _db.Categories.ToListAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, int catId)
        {
            ViewBag.Categories = await _db.Categories.ToListAsync();

            #region Is Exist
            bool isExist = await _db.Products.AnyAsync(x => x.Name == product.Name);
            if (isExist)
            {
                ModelState.AddModelError("Name", "This product is already exist");
                return View();
            }
            #endregion

            #region Save Image
            if (product.Photo == null)
            {
                ModelState.AddModelError("Photo", "please select photo");
                return View();
            }
            if (!product.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "please select image type");
                return View();
            }
            if (product.Photo.IsOrder1Mb())
            {
                ModelState.AddModelError("Photo", "Max 1Mb");
                return View();
            }
            string folder = Path.Combine(_env.WebRootPath, "img");
            product.Image = await product.Photo.SaveFileAsync(folder);
            #endregion

            product.CategoryId = catId;

            await _db.Products.AddAsync(product);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product dbProduct = await _db.Products.FirstOrDefaultAsync();

            if (dbProduct == null)
            {
                return BadRequest();
            }

            ViewBag.Categories = await _db.Categories.ToListAsync();
            return View(dbProduct);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Product product, int catId)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product dbProduct = await _db.Products.FirstOrDefaultAsync();

            if (dbProduct == null)
            {
                return BadRequest();
            }

            ViewBag.Categories = await _db.Categories.ToListAsync();

            #region Is Exist
            bool isExist = await _db.Products.AnyAsync(x => x.Name == product.Name && x.Id != id);
            if (isExist)
            {
                ModelState.AddModelError("Name", "This product is already exist");
                return View();
            }
            #endregion

            #region Save Image
            if (product.Photo != null)
            {
                if (!product.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "please select image type");
                    return View();
                }
                if (product.Photo.IsOrder1Mb())
                {
                    ModelState.AddModelError("Photo", "Max 1Mb");
                    return View();
                }
                string folder = Path.Combine(_env.WebRootPath, "img");
                dbProduct.Image = await product.Photo.SaveFileAsync(folder);
            }
            #endregion

            dbProduct.Name= product.Name;
            dbProduct.Price = product.Price;
            dbProduct.CategoryId = catId;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
