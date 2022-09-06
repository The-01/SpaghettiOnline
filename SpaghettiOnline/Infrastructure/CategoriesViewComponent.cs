using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpaghettiOnline.Data;
using SpaghettiOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaghettiOnline.Infrastructure
{
    public class CategoriesViewComponent : ViewComponent
    {
        private readonly AppDbContext context;
        public CategoriesViewComponent(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await GetCategoriesAsync();

            return View(categories);
        }

        private Task<List<Category>> GetCategoriesAsync()
        {
            return context.Categories.OrderBy(x => x.DisplayOrder).ToListAsync();
        }
    }
}
