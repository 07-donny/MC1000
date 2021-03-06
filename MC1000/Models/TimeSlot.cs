﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MC1000.Models
{
    public class TimeSlot
    {
        public int Id { get; set; }
        [DataType(DataType.Time)] // Dit zorgt ervoor dat de tijd wordt displayed inplaats van de datum en tijd
        public DateTime StartTime { get; set; }
        [DataType(DataType.Time)] // Dit zorgt ervoor dat de tijd wordt displayed inplaats van de datum en tijd
        public DateTime EndTime { get; set; }
        public decimal Price { get; set; }

        public DeliverySlot DeliverySlot { get; set; }
        public int DeliverySlotId { get; set; }
    }
}
