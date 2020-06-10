using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MC1000.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime DatePlaced { get; set; }
        public string Status { get; set; }

        // Bijbehorende DeliverySlot
        public int DeliverySlotId { get; set; }

        public DeliverySlot DeliverySlot { get; set; }

        // Gekoppelde gebruiker
        public string UserId { get; set; }

        public User User { get; set; }

        public List<OrderLine> OrderLines { get; set; }
    }
}