using FiorellaFrontToBack.DateAccessLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiorellaFrontToBack.Areas.AdminPanel.Controllers
{
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
    }
}
