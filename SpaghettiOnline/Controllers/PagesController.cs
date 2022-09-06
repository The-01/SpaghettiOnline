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
    public class PagesController : Controller
    {
        private readonly AppDbContext context;
        public PagesController(AppDbContext context)
        {
            this.context = context;
        }

        //GET /(It is the root(In my case, https://localhost44329)) or /slug
        public async Task<IActionResult> Page(string slug)
        {
            if (slug == null || slug == "home")
            {
                return View("Homepage");
            }

            if (slug == "about")
            {
                return View("About");
            }

            if (slug == "contact")
            {
                return View("Contact");
            }

            Page page = await context.Pages.Where(x => x.Slug == slug).FirstOrDefaultAsync();

            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }

        //Get /slug=home
        public IActionResult Homepage() => View();

        //Get /slug=about
        public IActionResult About() => View();

        //Get /slug=contact
        public IActionResult Contact() => View();
    }
}
