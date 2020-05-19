using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MC1000.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        // Gekoppelde gebruiker
        public string UserId { get; set; }
        public User User { get; set; }

        public List<Product> Products { get; set; }
    }
}
