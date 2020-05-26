using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MC1000.Models
{
    public class SubCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BannerImage { get; set; }

        // Bijbehorende Category
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<SubSubCategory> SubSubCategories { get; set; }
    }
}
