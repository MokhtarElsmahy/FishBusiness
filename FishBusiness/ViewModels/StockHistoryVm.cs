using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.ViewModels
{
    public class StockHistoryVm
    {
        public string MerchantName { get; set; }
        public string ProductionType { get; set; }
        public double Qty { get; set; }
        public decimal price { get; set; }
    }
}
