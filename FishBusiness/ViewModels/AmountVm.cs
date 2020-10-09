using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.ViewModels
{
    public class AmountVm
    {
        public Guid? AmountId { get; set; }
        public IGrouping<Guid?, int> items { get; set; }
    }
}
