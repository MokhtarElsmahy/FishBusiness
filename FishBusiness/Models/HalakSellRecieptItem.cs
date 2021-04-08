using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.Models
{
    public class HalakSellRecieptItem
    {
        [Key]
        public int HalakSellRecieptItemID { get; set; }

        [ForeignKey("HalakSellReciep")]
        public int HalakSellRecieptID { get; set; }

        [Display(Name = "الكميه بالكجم/طوايل")]
        public double Qty { get; set; }

        [Display(Name = "بوكسات/طوايل")]
        [Required(ErrorMessage = "برجاء ادخال عدد بوكسات/طوايل")]
        public int BoxQty { get; set; }

        [ForeignKey("Fish")]
        public int FishID { get; set; }
        public virtual Fish Fish { get; set; }

        [Display(Name = "سعر الكيلو")]
        public decimal UnitPrice { get; set; }

        [ForeignKey("ProductionType")]
        public int ProductionTypeID { get; set; }
        public virtual ProductionType ProductionType { get; set; }
    }
}
