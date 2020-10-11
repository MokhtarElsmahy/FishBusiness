using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FishBusiness.Models;

namespace FishBusiness.ViewModels
{
    public class CheckoutVM
    {
        public IEnumerable<IncomesOfSharedBoat> incomesOfSharedBoats { get; set; }
        public IEnumerable<Expense> Expenses { get; set; }
    }
}
