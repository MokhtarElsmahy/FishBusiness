using FishBusiness.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.ViewModels
{
    public class OperatorProfileVm
    {
        public Operator Operator { get; set; }
        public ICollection<PaidForOperator> PaidsForOperator { get; set; }
        public ICollection<OperatorDeal> OperatorDeals { get; set; }
    }
}
