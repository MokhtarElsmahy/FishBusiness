using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.Models
{
    public class Debt
    {
        [Key]
        public int DebtID { get; set; }

        [Required(ErrorMessage ="ادخل نوع الهالك")]
        [Display(Name ="نوع الهالك")]
        public string DebtName { get; set; }
        public virtual ICollection<Debts_Sarha> Debts_Sarhas { get; set; }
    }
}
