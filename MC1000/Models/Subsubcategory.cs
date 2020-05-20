using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MC1000.Models
{
    public class SubSubCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BannerImage { get; set; }

        // Bijbehorende SubCategory
        public int SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }
    }
}
