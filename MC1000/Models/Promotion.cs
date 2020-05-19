﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MC1000.Models
{
    public class Promotion
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public List<Discount> Discounts { get; set; }
    }
}
