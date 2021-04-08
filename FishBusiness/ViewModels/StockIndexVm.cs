using FishBusiness.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.ViewModels
{
    public class StockIndexVm
    {
        public List<Stock> Stocks { get; set; }
        public List<StockHistory> StockHistory { get; set; }
    }
}
