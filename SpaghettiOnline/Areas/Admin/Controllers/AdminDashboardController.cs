using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class AdminDashboardController : Controller
    {
        private readonly AppDbContext context;
        public AdminDashboardController(AppDbContext context)
        {
            this.context = context;
        }

        //GET : Admin/AdminDashboard
        public IActionResult Index()
        {
            ViewBag.TotalPages = context.Pages.Count();
            ViewBag.TotalCategories = context.Categories.Count();
            ViewBag.TotalProducts = context.Products.Count();
            ViewBag.TotalUsers = context.Users.Count();
            ViewBag.Roles = context.Roles.Count();

            return View();
        }
    }
}
