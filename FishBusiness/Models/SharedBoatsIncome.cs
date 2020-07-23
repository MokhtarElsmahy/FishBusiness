using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.Models
{
    public class SharedBoatsIncome
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("Boat")]
        public int BoatID { get; set; }
        public virtual Boat Boat { get; set; }

       
        [Display(Name = "الايراد")]
        public decimal Income { get; set; }
        

        

     
        [Display(Name = "تاريخ التحصيل")]
        public DateTime Date { get; set; }
    }
}
