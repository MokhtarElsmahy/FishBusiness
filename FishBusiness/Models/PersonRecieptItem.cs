using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.Models
{
    public class PersonRecieptItem
    {
        public int PersonRecieptItemID { get; set; }

        [Display(Name = "الكمية")]
        [Required(ErrorMessage = "برجاء ادخال الكمية")]
        public double Qty { get; set; }

        [Display(Name = "سعر الوحدة")]
        [Required(ErrorMessage = "برجاء ادخال سعر الوحدة")]
        public decimal UnitPrice { get; set; }

        [ForeignKey("ProductionType")]
        public int ProductionTypeID { get; set; }
        public virtual ProductionType ProductionType { get; set; }
        public Guid? AmountId { get; set; }


        [ForeignKey("Boat")]
        public int BoatID { get; set; }
        public virtual Boat Boat { get; set; }

        [ForeignKey("Fish")]
        public int FishID { get; set; }
        public virtual Fish Fish { get; set; }


        [ForeignKey("PersonReciept")]
        public int PersonRecieptID { get; set; }
        public virtual PersonReciept PersonReciept { get; set; }
    }
}
