using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.Models
{
    public class Sarha
    {
        [Key]
        public int SarhaID { get; set; }
        [ForeignKey("Boat")]
        public int BoatID { get; set; }

       
        [Required(ErrorMessage ="ادخل عدد الانفار")]
        [Display(Name ="عدد الانفار")]
        public int NumberOfFishermen { get; set; }

        [Display(Name = "عدد الطوايل المطلوبة")]
        public int  NumberOfBoxes { get; set; }

        [Required(ErrorMessage = "ادخل تاريخ السرحه")]
        [Display(Name = "تاريخ السرحه")]
        public DateTime DateOfSarha { get; set; }

        public virtual Boat Boat { get; set; }

        public virtual ICollection<BoatOwnerReciept> BoatOwnerReciepts { get; set; }
        public virtual ICollection<Fisherman> Fishermen { get; set; }

        public virtual ICollection<Debts_Sarha> Debts_Sarhas { get; set; }
    }
}
