using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.Models
{
    public class IncomesOfSharedBoat
    {
        public int ID { get; set; }

        [Display(Name ="التاريخ")]
        public DateTime Date { get; set; }


        [Display(Name = "الايراد")]
        public decimal Income { get; set; }
        public bool IsCheckedOut { get; set; }

        public int BoatID { get; set; }

        public virtual Boat Boat { get; set; }
    }
}
