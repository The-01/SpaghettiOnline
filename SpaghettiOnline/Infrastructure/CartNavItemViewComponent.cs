using Microsoft.AspNetCore.Mvc;
using SpaghettiOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaghettiOnline.Infrastructure
{
    public class CartNavItemViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");
            CartNavItem cartNavItem;

            if (cart == null || cart.Count == 0)
            {
                cartNavItem = null;
            }
            else
            {
                cartNavItem = new CartNavItem
                {
                    NumberOfItems = cart.Sum(x => x.Quantity)
                };
            }

            return View(cartNavItem);
        }
    }
}
