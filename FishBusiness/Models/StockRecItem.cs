using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.Models
{
    public class StockRecItem
    {
        [Key]
        public int StockRecItemID { get; set; }

        [ForeignKey("StockRec")]
        public int StockRecID { get; set; }

        public virtual StockRec StockRec { get; set; }

        [ForeignKey("Fish")]
        public int FishID { get; set; }

        public virtual Fish Fish { get; set; }

        [Display(Name = "سعر الوحدة")]
        [Required(ErrorMessage = "برجاء ادخال سعر الوحدة")]
        public decimal UnitPrice { get; set; }

        public Guid? AmountId { get; set; }


        [Display(Name = "الكمية")]
        [Required(ErrorMessage = "برجاء ادخال الكمية")]
        public double Qty { get; set; }

        [ForeignKey("ProductionType")]
        public int ProductionTypeID { get; set; }
        public virtual ProductionType ProductionType { get; set; }
    }
}
