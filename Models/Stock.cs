using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Chondok.Models
{
    public class Stock
    {
        public int Id { get; set; }

        [Required]
        [Display(Name ="Amount of Stock")]
        public int StockQty { get; set; }

    }
}
