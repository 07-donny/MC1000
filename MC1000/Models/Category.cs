using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MC1000.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BannerImage { get; set; }

        public List<SubCategory> SubCategories { get; set; }
    }
}
