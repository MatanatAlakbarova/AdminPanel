using FiorellaFrontToBack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FiorellaFrontToBack.ViewModels
{
    public class HomeVM
    {
        public List<SliderImage> SliderImages { get; set; }
        public Slider Slider { get; set; }
        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }
        public AboutImage AboutImage { get; set; }
        public AboutText AboutText { get; set; }
        public ExpertContent ExpertContent { get; set; }
        public List<Expert> Experts { get; set; }
        public Devider Devider { get; set; }
        public BlogContent BlogContent { get; set; }
        public List<BlogCard> BlogCards { get; set; }
        public List<SayCarousel> SayCarousels { get; set; }
        public List<InstagramImage> InstagramImages { get; set; }
    }
}
