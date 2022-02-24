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
    public class SliderImageController : Controller
    {
        private readonly AppDbContext _dbContext;

        public SliderImageController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var sliderImages =  _dbContext.SliderImages.ToList();
            return View(sliderImages);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var image = await _dbContext.SliderImages.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }
            return View(image);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var image = await _dbContext.SliderImages.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }
            return View(image);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteImage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
           
            var existImage = await _dbContext.SliderImages.FindAsync(id);
            if (existImage == null)
            {
                return NotFound();
            }
          
            var path = Path.Combine(Constant.ImagePath, existImage.Name);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            _dbContext.SliderImages.Remove(existImage);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderImage sliderImage)
        {
            if (!ModelState.IsValid)
                return View();
            var uploadImageCount = sliderImage.Photos.Count();
            var ImageCount = _dbContext.SliderImages.Count();
            if (ImageCount+uploadImageCount>5)
            {
                ModelState.AddModelError("Photos", "5-den artiq shekil olmaz.");
                return View();
            }
            
            foreach (var photo in sliderImage.Photos)
            {
                if (!photo.IsImage())
                {
                    ModelState.AddModelError("Photo", $"{photo}Yuklediyiniz sekil olmalidir");
                    return View();
                }

                if (!photo.IsAllowedSize(1))
                {
                    ModelState.AddModelError("Photo", $"{photo} 1 mgb-dan az olmalidir");
                    return View();
                }
                var fileName = await photo.GenerateFile(Constant.ImagePath);
                var sliderImage2 = new SliderImage { Name = fileName };
                await _dbContext.SliderImages.AddAsync(sliderImage2);
                await _dbContext.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            var sliderImage = await _dbContext.SliderImages.FirstOrDefaultAsync(x => x.Id == id);
            if (sliderImage==null)
            {
                return NotFound();
            }
            return View(sliderImage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id,SliderImage sliderImage)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (id!=sliderImage.Id)
            {
                return BadRequest();
            }
            var existImage = await _dbContext.SliderImages.FindAsync(id);
            if (existImage==null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(existImage);
            }
            if (!sliderImage.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", $"{sliderImage.Photo.FileName}- sekil olmalidir");
                return View();
            }

            if (!sliderImage.Photo.IsAllowedSize(1))
            {
                ModelState.AddModelError("Photo", $"{sliderImage.Photo.FileName} 1 mgb-dan az olmalidir");
                return View();
            }
            var path = Path.Combine(Constant.ImagePath, existImage.Name);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            var fileName = await sliderImage.Photo.GenerateFile(Constant.ImagePath);
            existImage.Name = fileName;
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
