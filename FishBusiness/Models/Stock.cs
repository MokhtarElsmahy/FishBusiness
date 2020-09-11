using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.Models
{
    public class Stock
    {
        [Key]
        public int StockID { get; set; }

        [ForeignKey("Fish")]
        public int FishID { get; set; }

        public virtual Fish Fish { get; set; }


       
        [Display(Name = "الكمية")]
        [Required(ErrorMessage = "برجاء ادخال الكمية")]
        public double Qty { get; set; }

        [ForeignKey("ProductionType")]
        public int ProductionTypeID { get; set; }
        public virtual ProductionType ProductionType { get; set; }

        [Display(Name = "الوزن الاجمالى")]
        public double TotalWeight { get; set; }

        public DateTime Date { get; set; }
        public bool FirstTimeFlag { get; set; }
    }
}
