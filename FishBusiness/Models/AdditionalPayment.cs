using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.Models
{
    public class AdditionalPayment
    {
        [Key]
        public int AdditionalPaymentID { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }

        [ForeignKey("Collecting")]
        public int ID { get; set; }
        public DateTime Date { get; set; }

        public virtual Collecting Collecting { get; set; }
    }
}
