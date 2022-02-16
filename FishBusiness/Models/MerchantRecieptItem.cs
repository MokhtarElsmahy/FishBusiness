using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.Models
{
    public class MerchantRecieptItem
    {

        public int MerchantRecieptItemID { get; set; }

        [Display(Name = "الكمية")]
        [Required(ErrorMessage = "برجاء ادخال الكمية")]
        public double Qty { get; set; }

        [Display(Name = "سعر الوحدة")]
        [Required(ErrorMessage = "برجاء ادخال سعر الوحدة")]
        public decimal UnitPrice { get; set; }

        public Guid? AmountId { get; set; }

        [ForeignKey("ProductionType")]
        public int ProductionTypeID { get; set; }
        public virtual ProductionType ProductionType { get; set; }


        [ForeignKey("Boat")]
        public int? BoatID { get; set; }
        public virtual Boat Boat { get; set; }

        [ForeignKey("Fish")]
        public int FishID { get; set; }
        public virtual Fish Fish { get; set; }


        [ForeignKey("MerchantReciept")]
        public int MerchantRecieptID { get; set; }
        public virtual MerchantReciept MerchantReciept { get; set; }


    }
}
