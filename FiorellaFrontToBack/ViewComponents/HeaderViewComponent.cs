using FiorellaFrontToBack.DateAccessLayer;
using FiorellaFrontToBack.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiorellaFrontToBack.ViewComponents
{
    public class HeaderViewComponent:ViewComponent
    {
        private readonly AppDbContext _dbContext;
        public HeaderViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var count = 0;
            var basket = Request.Cookies["basket"];
            var products = JsonConvert.DeserializeObject<List<BasketViewModel>>(basket);
            if (!string.IsNullOrEmpty(basket))
            {
                count = products.Count;
            }
            ViewBag.BasketCount = count;
            double totalAmount = 0;
            foreach (var item in products)
            {
                totalAmount += item.Price * item.Count;
            }
            ViewBag.BasketTotalAmount = totalAmount;

            var bio = await _dbContext.Bios.SingleOrDefaultAsync();
            return View(bio);
        }
    }
}
