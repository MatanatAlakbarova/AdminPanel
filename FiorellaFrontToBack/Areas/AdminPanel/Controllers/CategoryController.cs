using FiorellaFrontToBack.Data;
using FiorellaFrontToBack.DateAccessLayer;
using FiorellaFrontToBack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiorellaFrontToBack.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _dbContext;

        public CategoryController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var categories = await _dbContext.Categories.ToListAsync();
            return View(categories);


        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id==null)
            {
                return BadRequest();
            }
            var category = await _dbContext.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Category category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var isExsitCategory = await _dbContext.Categories.AnyAsync(x => x.Name.ToLower().Trim() == category.Name.ToLower().Trim());
            if (isExsitCategory)
            {
                ModelState.AddModelError("Name", "This name already avaible");
                return View();
            }
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id==null)
            {
                return BadRequest();
            }
            var category =await _dbContext.Categories.FindAsync(id);
            if (category==null)
            {
                return NotFound();
            }
            _dbContext.Categories.Remove(category);
            _dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));


        }
    }
}
