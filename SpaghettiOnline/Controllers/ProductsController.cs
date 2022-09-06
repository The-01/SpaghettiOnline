using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpaghettiOnline.Data;
using SpaghettiOnline.Infrastructure;
using SpaghettiOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaghettiOnline.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext context;
        public ProductsController(AppDbContext context)
        {
            this.context = context;
        }

        //GET : /Products
        public async Task<IActionResult> Index(int p = 1)
        {
            const int pageSize = 8;

            if (p < 1)
            {
                p = 1;
            }

            int countItems = context.Products.Count();
            
            var pageBar = new PaginationBar(countItems, p, pageSize);

            int skipItems = (p - 1) * pageSize;

            var products = context.Products.Skip(skipItems).Take(pageBar.PageSize);

            ViewBag.PaginationBar = pageBar;

            return View(await products.OrderByDescending(x => x.Id).ToListAsync());
        }

        //GET : /Products/Category=?
        public async Task<IActionResult> ProductsByCategory(string categorySlug, int p = 1)
        {
            Category category = await context.Categories.Where(x => x.Slug == categorySlug).FirstOrDefaultAsync();

            if (category == null)
            {
                return RedirectToAction("Index");
            }

            const int pageSize = 9;

            if (p < 1)
            {
                p = 1;
            }

            int countItems = context.Products.Count();

            var pageBar = new PaginationBar(countItems, p, pageSize);

            int skipItems = (p - 1) * pageSize;

            var products = context.Products.Where(x => x.CategoryId == category.Id).Skip(skipItems).Take(pageBar.PageSize);

            ViewBag.PaginationBar = pageBar;
            ViewBag.CategoryName = category.Name;

            return View(await products.OrderByDescending(x => x.Id).ToListAsync());
        }
    }
}
