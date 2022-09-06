using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpaghettiOnline.Data;
using SpaghettiOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaghettiOnline.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly AppDbContext context;
        public CategoriesController(AppDbContext context)
        {
            this.context = context;
        }

        //GET : Admin/Categories
        public async Task<IActionResult> Index()
        {
            return View(await context.Categories.OrderBy(x => x.DisplayOrder).ToListAsync());
        }

        //GET : Admin/Categories/Create
        public IActionResult Create() => View();

        //POST : Admin/Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                category.Slug = category.Name.ToLower().Replace(" ", "-");
                category.DisplayOrder = 100;

                var slug = await context.Categories.FirstOrDefaultAsync(x => x.Slug == category.Slug);

                if (slug != null)
                {
                    ModelState.AddModelError("", "Category already exist!");
                    return View(category);
                }

                context.Add(category);
                await context.SaveChangesAsync();
                TempData["success"] = "The Category has been added successfully!";

                return RedirectToAction("Index");
            }

            return View(category);
        }

        //GET : Admin/Categories/Edit/id=?
        public async Task<IActionResult> Edit(int id)
        {
            Category category = await context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        //POST : Admin/Categories/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (ModelState.IsValid)
            {
                category.Slug = category.Name.ToLower().Replace(" ", "-");

                var slug = await context.Categories.Where(x => x.Id != id).FirstOrDefaultAsync(x => x.Slug == category.Slug);

                if (slug != null)
                {
                    ModelState.AddModelError("", "Category already exist!");
                    return View(category);
                }

                context.Update(category);
                await context.SaveChangesAsync();
                TempData["success"] = "The Category has been edited successfully!";

                return RedirectToAction("Edit", new { id });
            }

            return View(category);
        }

        //GET : Admin/Categories/Delete/id=?
        public async Task<IActionResult> Delete(int id)
        {
            Category category = await context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        //POST : Admin/Categories/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            Category category = await context.Categories.FindAsync(id);

            if (category == null)
            {
                TempData["error"] = "Category does not exist!";
            }
            else
            {
                context.Categories.Remove(category);
                await context.SaveChangesAsync();
                TempData["success"] = "The Category has been deleted successfully!";
            }

            return RedirectToAction("Index");
        }

        //POST : Admin/Categories/Reorder
        [HttpPost]
        public async Task<IActionResult> Reorder(int[] id)
        {
            int count = 1;

            foreach (var categoryId in id)
            {
                Category category = await context.Categories.FindAsync(categoryId);
                category.DisplayOrder = count;
                context.Update(category);
                await context.SaveChangesAsync();
                count++;
            }

            return Ok();
        }
    }
}
