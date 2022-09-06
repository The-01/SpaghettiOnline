using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SpaghettiOnline.Data;
using SpaghettiOnline.Infrastructure;
using SpaghettiOnline.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SpaghettiOnline.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment env;
        public ProductsController(AppDbContext context, IWebHostEnvironment env)
        {
            this.context = context;
            this.env = env;
        }

        //GET : Admin/Products
        public async Task<IActionResult> Index()
        {
            return View(await context.Products.OrderByDescending(x => x.Id).Include(x => x.Category).ToListAsync());
        }

        //GET : Admin/Products/Details/id=?
        public async Task<IActionResult> Details(int id)
        {
            Product product = await context.Products.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        //GET : Admin/Products/Create
        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.DisplayOrder), "Id", "Name");

            return View();
        }

        //POST : Admin/Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.DisplayOrder), "Id", "Name");

            if (ModelState.IsValid)
            {
                product.Slug = product.Name.ToLower().Replace(" ", "-");

                var slug = await context.Products.FirstOrDefaultAsync(x => x.Slug == product.Slug);

                if (slug != null)
                {
                    ModelState.AddModelError("", "The Product already exist!");
                    return View(product);
                }

                string imgUrl = "default.png";

                if (product.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(env.WebRootPath, "media/products");
                    imgUrl = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imgUrl);
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                }

                product.Image = imgUrl;

                context.Add(product);
                await context.SaveChangesAsync();
                TempData["success"] = "The Product has been added successfully!";

                return RedirectToAction("Index");
            }

            return View(product);
        }

        //GET : Admin/Products/Edit/id=?
        public async Task<IActionResult> Edit(int id)
        {
            Product product = await context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.DisplayOrder), "Id", "Name", product.CategoryId);

            return View(product);
        }

        //POST : Admin/Products/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.DisplayOrder), "Id", "Name", product.CategoryId);

            if (ModelState.IsValid)
            {
                product.Slug = product.Name.ToLower().Replace(" ", "-");

                var slug = await context.Products.Where(x => x.Id != id).FirstOrDefaultAsync(x => x.Slug == product.Slug);

                if (slug != null)
                {
                    ModelState.AddModelError("", "The Product already exist!");
                    return View(product);
                }

                if (product.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(env.WebRootPath, "media/products");

                    if (!string.Equals(product.Image, "default.png"))
                    {
                        string oldImgUrl = Path.Combine(uploadsDir, product.Image);

                        if (System.IO.File.Exists(oldImgUrl))
                        {
                            System.IO.File.Delete(oldImgUrl);
                        }
                    }

                    string imgUrl = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imgUrl);
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    product.Image = imgUrl;
                }

                context.Update(product);
                await context.SaveChangesAsync();
                TempData["success"] = "The Product has been edited successfully!";

                return RedirectToAction("Index");
            }

            return View(product);
        }

        //GET : Admin/Products/Delete/id=?
        public async Task<IActionResult> Delete(int id)
        {
            Product product = await context.Products.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.DisplayOrder), "Id", "Name", product.CategoryId);

            return View(product);
        }

        //POST : Admin/Prdoucts/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            Product product = await context.Products.FindAsync(id);

            if (product == null)
            {
                TempData["error"] = "Product does not exist!";
            }
            else
            {

                if (!string.Equals(product.Image, "default.png"))
                {
                    string uploadsDir = Path.Combine(env.WebRootPath, "media/products");
                    string imgPath = Path.Combine(uploadsDir, product.Image);

                    if (System.IO.File.Exists(imgPath))
                    {
                        System.IO.File.Delete(imgPath);
                    }
                }

                context.Products.Remove(product);
                await context.SaveChangesAsync();
                TempData["success"] = "The Product has been deleted successfully!";
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GenerateReport(int param)
        {
            List<Product> productsList = new List<Product>();
            var products = await context.Products.Include(x => x.Category).ToListAsync();

            if (products?.Any() == true)
            {
                foreach (var product in products)
                {
                    productsList.Add(new Product
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Category = product.Category,
                        Price = product.Price
                    });
                }
            }

            ProductReport report = new ProductReport(context, env);

            return File(report.Report(productsList), "application/pdf");
        }
    }
}
