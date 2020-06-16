using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace MC1000.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string EAN { get; set; }
        public string Title { get; set; }
        public string Brand { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public string Image { get; set; }
        public string Weight { get; set; }

        [AllowNull]
        public double Price { get; set; }

        //public List<Discount> Discounts { get; set; }

        //public SubSubCategory SubSubCategory { get; set; }
        public string SubSub { get; set; }
    }
}