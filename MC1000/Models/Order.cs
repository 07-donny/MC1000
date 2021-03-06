﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
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
        public int TimeSlotId { get; set; }

        public TimeSlot TimeSlot { get; set; }

        // Gekoppelde gebruiker
        public string UserId { get; set; }

        public User User { get; set; }

        [AllowNull]
        [DataType(DataType.Currency)]
        public decimal TotalPrice { get; set; }

        public List<OrderLine> OrderLines { get; set; }

        internal Task Include()
        {
            throw new NotImplementedException();
        }
    }
}