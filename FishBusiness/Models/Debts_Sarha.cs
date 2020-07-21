using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.Models
{
    public class Debts_Sarha
    {
        [ForeignKey("Sarha")]
        public int SarhaID { get; set; }

        [ForeignKey("Debt")]
        public int DebtID { get; set; }

        [Display(Name ="سعر الهالك")]
        [Required(ErrorMessage ="برجاء ادخال سعر الهالك")]
        public decimal Price { get; set; }

        public virtual Sarha Sarha { get; set; }
        public virtual Debt Debt { get; set; }
    }
}
