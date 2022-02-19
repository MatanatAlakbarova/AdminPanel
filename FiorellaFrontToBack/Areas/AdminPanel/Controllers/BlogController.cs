using FiorellaFrontToBack.DateAccessLayer;
using FiorellaFrontToBack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiorellaFrontToBack.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class BlogController : Controller
    {
        private readonly AppDbContext _dbContext;

        public BlogController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var blogCards = await _dbContext.BlogCards.ToListAsync();
            return View(blogCards);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if ( id==null)
            {
                return BadRequest();
            }
            var blogCard = await _dbContext.BlogCards.FindAsync(id);
            if (blogCard==null)
            {
                return NotFound();
            }
            return View(blogCard);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogCard blogCard)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var isExsistBlogCard = await _dbContext.BlogCards.AnyAsync(x => x.Image == blogCard.Image);
            if (isExsistBlogCard)
            {
                ModelState.AddModelError("Image", "This Image already avaible");
                return View();
            }
           await _dbContext.BlogCards.AddAsync(blogCard);
           await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var card = await _dbContext.BlogCards.FindAsync(id);
            if (card == null)
            {
                return NotFound();
            }
            _dbContext.BlogCards.Remove(card);
            _dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));


        }
    }
}
