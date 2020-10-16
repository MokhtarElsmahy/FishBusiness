using FishBusiness.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.ViewModels
{
    public class StockDistributionVm
    {
        public StockRec StockRec { get; set; }
        public List<StockRecItem> NormalStockRecItems { get; set; }
        public List<StockRecItem> AmountStockRecItems { get; set; }
        public IEnumerable<AmountVm> Amounts { get; set; }
    }
}
