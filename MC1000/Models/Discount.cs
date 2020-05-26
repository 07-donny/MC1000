using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MC1000.Models
{
    public class Discount
    {
        public int Id { get; set; }
        public decimal DiscountedPrice { get; set; }
        public DateTime ValidUntil { get; set; }
        public Promotion Promotion { get; set; }
        public int PromotionId { get; set; }
        public Product Product { get; set; }
        public string EAN { get; set; }
    }
}
