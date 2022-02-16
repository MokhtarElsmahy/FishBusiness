using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FishBusiness.Models;

namespace FishBusiness.ViewModels
{
    public class SummationVm
    {
        public ICollection<IMerchantReciept> IMerchantReciepts { get; set; }
        public ICollection<ISellerReciept> ISellerReciepts { get; set; }
        public ICollection<HalakaBuyReciept> HalakaBuyReciepts { get; set; }
        public decimal  Labour { get; set; }
        public decimal Ice { get; set; }
    }
}
