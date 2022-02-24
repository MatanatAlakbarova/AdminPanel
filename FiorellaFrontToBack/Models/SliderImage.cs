using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FiorellaFrontToBack.Models
{
    public class SliderImage
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [NotMapped]
        public IFormFile Photo { get; set; }

        [NotMapped]
        public IFormFile[] Photos { get; set; }
    }
}
