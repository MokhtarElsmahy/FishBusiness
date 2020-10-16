using FishBusiness.Data.Migrations;
using FishBusiness.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.ViewModels
{
    public class DistributeSellerRec
    {
        public SellerRec SellerRec { get; set; }
        public List<SellerRecItem> NormalSellerRecItems { get; set; }
        public List<SellerRecItem> AmountSellerRecItems { get; set; }
        public IEnumerable<AmountVm> Amounts { get; set; }
    }
}
