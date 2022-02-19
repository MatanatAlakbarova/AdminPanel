using FiorellaFrontToBack.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiorellaFrontToBack.DateAccessLayer
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext>options):base(options)
        {
        }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<SliderImage> SliderImages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<AboutImage> AboutImages { get; set; }
        public DbSet<AboutText> AboutTexts { get; set; }
        public DbSet<ExpertContent> ExpertContents { get; set; }
        public DbSet<Expert> Experts { get; set; }
        public DbSet<Devider> Deviders { get; set; }
        public DbSet<BlogContent> BlogContents { get; set; }
        public DbSet<BlogCard> BlogCards { get; set; }
        public DbSet<SayCarousel> SayCarousels { get; set; }
        public DbSet <InstagramImage> InstagramImages { get; set; }
        public DbSet<Bio> Bios { get; set; }
    }
}
