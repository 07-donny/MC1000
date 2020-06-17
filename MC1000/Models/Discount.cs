using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MC1000.Models
{
    public class Discount
    {
        public int Id { get; set; }

        [DataType(DataType.Currency)]
        public decimal DiscountedPrice { get; set; }

        public DateTime ValidUntil { get; set; }
        public Promotion Promotion { get; set; }
        public int PromotionId { get; set; }
        public string EAN { get; set; }
        public string ImageURL { get; set; }

        //[ForeignKey("Product")]
        //public int ProductId { get; set; }

        //public Product Product { get; set; }
    }
}