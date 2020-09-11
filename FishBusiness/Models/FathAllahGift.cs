using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.Models
{
    public class FathAllahGift
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public decimal charge { get; set; }

        public decimal CreditBefore { get; set; }
        public decimal CreditAfter { get; set; }

        [ForeignKey("Person")]
        public int PersonID { get; set; }
        public Person Person { get; set; }
    }
}
