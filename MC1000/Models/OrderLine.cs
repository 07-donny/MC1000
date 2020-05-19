using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MC1000.Models
{
    public class OrderLine
    {
        public int Id { get; set; }

        // Bijbehorend Product
        public int ProductId { get; set; }
        public Product Product { get; set; }

        // Bijbehorende Promotion, kan leeg zijn
        public int PromotionId { get; set; }
        public Promotion Promotion { get; set; }

        public int Amount { get; set; }

        // Bijbehorende Order
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
