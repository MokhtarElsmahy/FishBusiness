using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.Models
{
    public class LeaderPayback
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }

        [ForeignKey("Boat")]
        public int BoatID { get; set; }
        public virtual Boat Boat { get; set; }
    }
}
