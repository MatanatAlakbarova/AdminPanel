using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FiorellaFrontToBack.Models
{
    public class BlogCard
    {
        public int Id { get; set; }
        [Required]
        public string Image { get; set; }
        public DateTime Time { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Paragraph { get; set; }
    }
}
