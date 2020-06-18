using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MC1000.Models
{
    public class CartItemViewModel
    {
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public int Amount { get; set; }
        public decimal TotalPrice => Price * Amount;
    }
}