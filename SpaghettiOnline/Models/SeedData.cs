using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SpaghettiOnline.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaghettiOnline.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new AppDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()))
            {
                if (context.Pages.Any())
                {
                    return;
                }

                context.Pages.AddRange(
                    new Page
                    {
                        Title = "Home",
                        Slug = "home",
                        Content = "This is home page",
                        DisplayOrder = 0
                    },
                    new Page
                    {
                        Title = "Categories",
                        Slug = "categories",
                        Content = "This is categories page",
                        DisplayOrder = 1
                    },
                    new Page
                    {
                        Title = "About",
                        Slug = "about",
                        Content = "This is about page",
                        DisplayOrder = 2
                    },
                    new Page
                    {
                        Title = "Contact",
                        Slug = "contact",
                        Content = "This is contact page",
                        DisplayOrder = 3
                    }
                    );

                context.SaveChanges();
            }
        }
    }
}
