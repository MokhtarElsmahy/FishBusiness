using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FishBusiness.Models;

namespace FishBusiness.ViewModels
{
    public class BranchOfficeVM
    {
        // رصيد مرحل
        public decimal CurrentCredit { get; set; }
        // تحصيل المينا
        public decimal Collecting { get; set; }
        public List<ISellerReciept> IsellerReciepts { get; set; }
        public virtual ICollection<Debts_Sarha> Debts_Sarha { get; set; }
        public virtual ICollection<HalekDifference> HalekDifferences { get; set; }
        public virtual ICollection<PaidForSeller> PaidForSellers { get; set; }
        public virtual ICollection<PaidForBoat> PaidForBoats { get; set; }
        public virtual ICollection<PaidForMerchant> PaidForMerchants { get; set; }

    }
}
