using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.Models
{
    public class Person
    {
        [Key]
        public int PersonID { get; set; }

        [Display(Name = "الاسم")]
        public string Name { get; set; }

        [Display(Name = "الرصيد")]
        public decimal credit { get; set; }

        public virtual ICollection<Debts_Sarha> Debts_Sarhas { get; set; }
        public virtual ICollection<FathAllahGift> FathAllahGifts { get; set; }
        public virtual ICollection<PaidForMerchant> PaidForMerchants { get; set; }
        public virtual ICollection<PaidForOperator> PaidForOperators { get; set; }
        public virtual ICollection<PaidForSeller> PaidForSellers { get; set; }
        public virtual ICollection<Expense> Expenses { get; set; }
        public virtual ICollection<OperatorDeal> OperatorDeals { get; set; }
        public virtual ICollection<BoatOwnerReciept> BoatOwnerReciepts { get; set; }
        public virtual ICollection<ISellerReciept> ISellerReciepts { get; set; }
        public virtual ICollection<LeaderLoan> LeaderLoans { get; set; }
        public virtual ICollection<PaidForBoat> PaidForBoats { get; set; }
        public virtual ICollection<AdditionalForOffice> AdditionalForOffices { get; set; }


    }
}
