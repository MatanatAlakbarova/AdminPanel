using FiorellaFrontToBack.DateAccessLayer;
using FiorellaFrontToBack.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiorellaFrontToBack.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _dbContext;

        public HomeController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var sliderImages = _dbContext.SliderImages.ToList();
            var slider = _dbContext.Sliders.SingleOrDefault();
            var categories = _dbContext.Categories.ToList();
           // var products = _dbContext.Products.Include(x=> x.Category).Take(4).ToList();
            var aboutImage = _dbContext.AboutImages.SingleOrDefault();
            var aboutText = _dbContext.AboutTexts.SingleOrDefault();
            var expertContent = _dbContext.ExpertContents.SingleOrDefault();
            var experts = _dbContext.Experts.ToList();
            var devider = _dbContext.Deviders.SingleOrDefault();
            var blogContent = _dbContext.BlogContents.SingleOrDefault();
            var blogCards = _dbContext.BlogCards.ToList();
            var saycorousels = _dbContext.SayCarousels.ToList();
            var instagramImages = _dbContext.InstagramImages.ToList();

            return View(new HomeVM
            {
               Slider=slider,
               SliderImages=sliderImages,
               Categories=categories,
              // Products=products,
               AboutImage=aboutImage,
               AboutText=aboutText,
               ExpertContent=expertContent,
               Experts=experts,
               Devider=devider,
               BlogCards=blogCards,
               BlogContent=blogContent,
               SayCarousels=saycorousels,
               InstagramImages=instagramImages

            });
        }
        public async Task<IActionResult> Search(string searchedProduct)
        {
            if (string.IsNullOrEmpty(searchedProduct))
            {
                return NoContent();
            }

            var products = await _dbContext.Products
                .Where(x => x.Name.ToLower().Contains(searchedProduct.ToLower()))
                .ToListAsync();

            return PartialView("_SeachedProductPartial", products);
        }

        public async Task<IActionResult> Basket()
        {
            var basket = Request.Cookies["Basket"];
            if (string.IsNullOrEmpty(basket))
            {
                return Content("empty");
            }
            var basketViewModels = JsonConvert.DeserializeObject<List<BasketViewModel>>(basket);
            var newBasket = new List<BasketViewModel>();
            foreach (var basketViewModel in basketViewModels)
            {
                var product = await _dbContext.Products.FindAsync(basketViewModel.Id);
                if (product == null)
                    continue;
                newBasket.Add(new BasketViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Image = product.Image,
                    Count = basketViewModel.Count,
                    TotalPrice = basketViewModel.Count * product.Price
                });
            }
            var basketList = JsonConvert.SerializeObject(newBasket);
            Response.Cookies.Append("basket", basketList);
            return View(newBasket);
        }
        public IActionResult Delete(int? id)
        {
            var basket = Request.Cookies["Basket"];
            if (string.IsNullOrEmpty(basket))
            {
                return Content("Empty");
            }
            var basketViewModels = JsonConvert.DeserializeObject<List<BasketViewModel>>(basket);
            var isAvailable = basketViewModels.FirstOrDefault(x => x.Id == id);
            if (isAvailable != null)
            {
                basketViewModels.Remove(isAvailable);
            }
            var basketList = JsonConvert.SerializeObject(basketViewModels);
            Response.Cookies.Append("Basket", basketList);
            return RedirectToAction(nameof(basket));
        }

        public IActionResult Increment(int? Id)
        {
            var basket = Request.Cookies["Basket"];
            if (string.IsNullOrEmpty(basket))
            {
                return Content("empty");
            }
            var basketViewModels = JsonConvert.DeserializeObject<List<BasketViewModel>>(basket);
            var isAvailable = basketViewModels.FirstOrDefault(x => x.Id == Id);
            if (isAvailable != null)
            {
                isAvailable.Count++;
            }
            var basketList = JsonConvert.SerializeObject(basketViewModels);
            Response.Cookies.Append("Basket", basketList);

            return RedirectToAction(nameof(basket));
        }
        public IActionResult Decrement(int? Id)
        {
            var basket = Request.Cookies["Basket"];
            if (string.IsNullOrEmpty(basket))
            {
                return Content("Emptyy");
            }
            var basketViewModels = JsonConvert.DeserializeObject<List<BasketViewModel>>(basket);
            var isAvailable = basketViewModels.FirstOrDefault(x => x.Id == Id);
            if (isAvailable.Count > 1)
            {
                isAvailable.Count--;
            }

            var basketList = JsonConvert.SerializeObject(basketViewModels);
            Response.Cookies.Append("Basket", basketList);

            return RedirectToAction(nameof(basket));
        }
        public async Task<IActionResult> AddToBasket(int? Id)
        {
            if (Id == null)
                return BadRequest();

            var Product = await _dbContext.Products.FindAsync(Id);
            if (Product == null)
                return NotFound();

            List<BasketViewModel> BasketViewModels;
            var basket = Request.Cookies["basket"];
            if (string.IsNullOrEmpty(basket))
            {
                BasketViewModels = new List<BasketViewModel>();
            }
            else
            {
                BasketViewModels = JsonConvert.DeserializeObject<List<BasketViewModel>>(basket);
            }
            var isAvailableBasketVM = BasketViewModels.FirstOrDefault(x => x.Id == Id);
            if (isAvailableBasketVM == null)
            {
                isAvailableBasketVM = new BasketViewModel
                {
                    Id = Product.Id,
                };
                BasketViewModels.Add(isAvailableBasketVM);
            }
            else
            {
                isAvailableBasketVM.Count++;
            }
            var Isbasket = JsonConvert.SerializeObject(BasketViewModels);
            Response.Cookies.Append("basket", Isbasket);

            return RedirectToAction(nameof(Index));
        }
    }
}