using System;
using System.Collections.Generic;
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
        public decimal Price { get; set; }

        //// Bijbehorende Category
        //public int CategoryId { get; set; }
        //public Category Category { get; set; }

        //// Bijbehorende SubCategory
        //public int SubCategoryId { get; set; }
        //public SubCategory SubCategory { get; set; }

        // Bijbehorende SubSubCategory
        public int SubSubCategoryId { get; set; }
        public SubSubCategory SubSubCategory { get; set; }
    }
}
