using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.Models
{
    public class HalakaBuyRecieptItem
    {
        [Key]
        public int HalakaBuyRecieptItemID { get; set; }

        [ForeignKey("HalakaBuyReciept")]
        public int HalakaBuyRecieptID { get; set; }

        public HalakaBuyReciept HalakaBuyReciept { get; set; }

        public Guid? AmountId { get; set; }

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
