using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MC1000.Models
{
    public class News
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        
        // Gekoppelde gebruiker
        public string UserId { get; set; }
        public string UserName { get; set; }
        public User User { get; set; }
    }
}
