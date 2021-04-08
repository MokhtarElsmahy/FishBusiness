using FishBusiness.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.ViewModels
{
    public class MerchantsIndexVmcs
    {
        public List<Merchant> InternalMerchants { get; set; }
        public List<Merchant> ExternalMerchants { get; set; }
    }
}
