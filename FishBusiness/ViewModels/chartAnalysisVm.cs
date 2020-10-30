using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.ViewModels
{
    public class chartAnalysisVm
    {
        public List<CommissionsVM> CommissionsVM { get; set; }
        public List<chartProfitVm> chatProfit { get; set; }
        public List<chartCollectionVm> chartCollectionVm { get; set; }
        public List<chartExternalCollectionVm> chartExternalCollectionVm { get; set; }
    }
}
