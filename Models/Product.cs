﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Chondok.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }
        public string Image { get; set; }

        [Display(Name ="Product Color")]
        public string ProductColor { get; set; }

        [Required]
        [Display(Name ="Available")]
        public bool IsAvailable { get; set; }

        [Required]
        [Display(Name="Number of Stocks")]
        public int StockInNo { get; set; }

        [Required]
        [Display(Name ="Product Type")]
        public int ProductTypeId { get; set; }

        [ForeignKey("ProductTypeId")]
        public ProductTypes ProductTypes { get; set; }

        [Required]
        [Display(Name ="Special Tag")]
        public int SpecialTagId { get; set; }

        [ForeignKey("SpecialTagId")]
        public SpecialTag SpecialTag { get; set; }

    }
}
