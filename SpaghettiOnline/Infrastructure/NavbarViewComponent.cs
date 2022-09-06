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
    public class NavbarViewComponent : ViewComponent
    {
        private readonly AppDbContext context;
        public NavbarViewComponent(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var pages = await GetPagesAsync();

            return View(pages);
        }

        private Task<List<Page>> GetPagesAsync()
        {
            return context.Pages.OrderBy(x => x.DisplayOrder).ToListAsync();
        }
    }
}
