using FishBusiness.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.ViewModels
{
    public class CollectorVm
    {
        public virtual ICollection<PaidForMerchant> PaidForMerchant { get; set; }
        public virtual ICollection<PaidForMerchant> PaidForUs { get; set; }
        public virtual ICollection<Debts_Sarha> Debts_Sarha { get; set; }
        public virtual ICollection<Expense> Expenses { get; set; }
    }
}
