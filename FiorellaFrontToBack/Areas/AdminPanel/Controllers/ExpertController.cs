using FiorellaFrontToBack.Areas.AdminPanel.Data;
using FiorellaFrontToBack.DateAccessLayer;
using FiorellaFrontToBack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FiorellaFrontToBack.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class ExpertController : Controller
    {
        private readonly AppDbContext _dbContext;

        public ExpertController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var experts = await _dbContext.Experts.ToListAsync();
            return View(experts);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var expert = await _dbContext.Experts.FindAsync(id);
            if (expert == null)
            {
                return NotFound();
            }
            return View(expert);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var expert = await _dbContext.Experts.FindAsync(id);
            if (expert == null)
            {
                return NotFound();
            }
            return View(expert);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteItem(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var existExpert = await _dbContext.Experts.FindAsync(id);
            if (existExpert == null)
            {
                return NotFound();
            }

            var path = Path.Combine(Constant.ImagePath, existExpert.Image);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            _dbContext.Experts.Remove(existExpert);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Expert expert)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var existExpert = _dbContext.Experts.FirstOrDefault(x => x.Name.Trim().ToLower() == expert.Name.Trim().ToLower());
            if (existExpert != null)
            {
                ModelState.AddModelError("Name", "Bu adda expert movcuddur.");
                return View();
            }
            if (!expert.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", $"{expert.Photo}Yuklediyiniz sekil olmalidir");
                return View();
            }

            if (!expert.Photo.IsAllowedSize(1))
            {
                ModelState.AddModelError("Photo", $"{expert.Photo} 1 mgb-dan az olmalidir");
                return View();
            }
            var fileName = await expert.Photo.GenerateFile(Constant.ImagePath);
            var newExpert = new Expert
            {
                Name = expert.Name,
                Image = fileName,
                Position = expert.Position

            };
            await _dbContext.Experts.AddAsync(newExpert);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var expert = await _dbContext.Experts.FirstOrDefaultAsync(x => x.Id == id);
            if (expert == null)
            {
                return NotFound();
            }
            return View(expert);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Expert expert)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (id != expert.Id)
            {
                return BadRequest();
            }
            var existExpert = await _dbContext.Experts.FindAsync(id);
            if (existExpert == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (expert.Photo==null)
            {
                existExpert.Name = expert.Name;
                existExpert.Position = expert.Position;
                await _dbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            if (!expert.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", $"{expert.Photo.FileName}- sekil olmalidir");
                return View();
            }

            if (!expert.Photo.IsAllowedSize(1))
            {
                ModelState.AddModelError("Photo", $"{expert.Photo.FileName} 1 mgb-dan az olmalidir");
                return View();
            }
            var path = Path.Combine(Constant.ImagePath, existExpert.Image);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            var fileName = await expert.Photo.GenerateFile(Constant.ImagePath);
            existExpert.Image = fileName;
            existExpert.Name = expert.Name;
            existExpert.Position = expert.Position;
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
