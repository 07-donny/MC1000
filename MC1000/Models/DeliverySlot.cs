using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MC1000.Models
{
    public class DeliverySlot
    {
        public int Id { get; set; }
        [DataType(DataType.Date)] // Dit zorgt ervoor dat de datum gedisplayed wordt als bijv "15-10-2020" ipv "15-10-2020 00:00:00"
        public DateTime DeliveryDate { get; set; }
        
        public List<TimeSlot> TimeSlots { get; set; }
    }
}
