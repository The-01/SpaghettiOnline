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
    [Authorize(Roles = "admin,moderator")]
    [Area("Admin")]
    public class PagesController : Controller
    {
        private readonly AppDbContext context;
        public PagesController(AppDbContext context)
        {
            this.context = context;
        }

        //GET : Admin/Pages
        public async Task<IActionResult> Index()
        {
            IQueryable<Page> pages = from p in context.Pages orderby p.DisplayOrder select p;
            List<Page> pagesList = await pages.ToListAsync();

            return View(pagesList);
        }

        //GET : Admin/Pages/Details/id=?
        public async Task<IActionResult> Details(int id)
        {
            Page page = await context.Pages.FirstOrDefaultAsync(p => p.Id == id);

            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }

        //GET : Admin/Pages/Create
        public IActionResult Create() => View();

        //POST : Admin/Pages/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Page page)
        {
            if (ModelState.IsValid)
            {
                page.Slug = page.Title.ToLower().Replace(" ", "-");
                page.DisplayOrder = 100;

                var slug = await context.Pages.FirstOrDefaultAsync(p => p.Slug == page.Slug);

                if (slug != null)
                {
                    ModelState.AddModelError("", "Page already exist!");
                    return View(page);
                }

                context.Add(page);
                await context.SaveChangesAsync();
                TempData["success"] = "The Page has been added successfully!";

                return RedirectToAction("Index");
            }

            return View(page);
        }

        //GET : Admin/Pages/Edit/id=?
        public async Task<IActionResult> Edit(int id)
        {
            Page page = await context.Pages.FindAsync(id);

            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }

        //POST : Admin/Pages/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Page page)
        {
            if (ModelState.IsValid)
            {
                page.Slug = page.Id == 1 ? "home" : page.Title.ToLower().Replace(" ", "-");

                var slug = await context.Pages.Where(p => p.Id != page.Id).FirstOrDefaultAsync(p => p.Slug == page.Slug);

                if (slug != null)
                {
                    ModelState.AddModelError("", "Page already exist!");
                    return View(page);
                }

                context.Update(page);
                await context.SaveChangesAsync();
                TempData["success"] = "The Page has been edited successfully!";

                return RedirectToAction("Edit", new { id = page.Id });
            }

            return View(page);
        }

        //GET : Admin/Pages/Delete/id=?
        public async Task<IActionResult> Delete(int id)
        {
            Page page = await context.Pages.FindAsync(id);

            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }

        //POST : Admin/Pages/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePage(int id)
        {
            Page page = await context.Pages.FindAsync(id);

            if (page == null)
            {
                TempData["error"] = "Page does not exist!";
            }
            else
            {
                context.Pages.Remove(page);
                await context.SaveChangesAsync();
                TempData["success"] = "The Page has been deleted successfully!";
            }

            return RedirectToAction("Index");
        }

        //POST : Admin/Pages/Reorder
        [HttpPost]
        public async Task<IActionResult> Reorder(int[] id)
        {
            int count = 1;

            foreach (var pageId in id)
            {
                Page page = await context.Pages.FindAsync(pageId);
                page.DisplayOrder = count;
                context.Update(page);
                await context.SaveChangesAsync();
                count++;
            }

            return Ok();
        }
    }
}
