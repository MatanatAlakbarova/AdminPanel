using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FiorellaFrontToBack.Models
{
    public class Bio
    {
        public int Id { get; set; }

        [Required]
        public string Logo { get; set; }

        [StringLength(100)]
        public string FacebookUrl { get; set; }

        [StringLength(100)]
        public string LinkedinUrl { get; set; }
    }
}
