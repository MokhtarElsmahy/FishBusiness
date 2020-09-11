using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.ViewModels
{
    public class MoneytizationVm
    {
        public int IsellerRecieptID { get; set; }
        public double TotalOfPrices { get; set; }
        public double Commision { get; set; }
        public double TotalOfPricesAfterCommision { get; set; }
        public double PaidFromDebt { get; set; }
        public double DebtsAfterCommisionAndPayment { get; set; }

        
                
               
             
    }
}
