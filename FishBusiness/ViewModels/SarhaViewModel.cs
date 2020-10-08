using FishBusiness.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.ViewModels
{
    public class SarhaViewModel
    {
        public Sarha Sarha { get; set; }
        public IEnumerable<Debt> Debts { get; set; }
        public IEnumerable<Debt> OtherDebts { get; set; }
        public IEnumerable<Debt_In_Sarha> Debts_Sarha { get; set; }
        public IEnumerable<Debts_Sarha> Debts_Sarhas { get; set; }
       
        public IEnumerable<Person> People { get; set; }
       

      
    }
}
