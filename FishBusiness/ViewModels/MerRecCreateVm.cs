using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.ViewModels
{
    public class MerRecCreateVm
    {
        public DateTime Date { get; set; }

        public int MerchantID { get; set; }



        public decimal TotalOfReciept { get; set; }

        public int RecID { get; set; }

        public decimal payment { get; set; }
        public decimal CurrentDebt { get; set; }

        public string FishNames { get; set; }
        public string ProductionTypes { get; set; }
        public string qtys { get; set; }
        public string unitprices { get; set; }
        public string boats { get; set; }
       

        //FishNames: FishNames,
        //                ProductionTypes: ProductionTypes,
        //                qtys: qtys,
        //                unitprices: unitprices,
        //                boats: boats
    }
}
