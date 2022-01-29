using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Chondok.Models
{
    public class SpecialTag
    {
        public int Id { get; set; }

        [Required]
        [Display(Name ="Tag Name")]
        public string TagName { get; set; }
    }
}
