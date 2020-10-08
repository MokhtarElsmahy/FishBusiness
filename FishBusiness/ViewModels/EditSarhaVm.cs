using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.ViewModels
{
    public class EditSarhaVm
    {
        public int BoatID { get; set; }
        public int NoFisherMen { get; set; }
        public int NoBoxes { get; set; }
        public DateTime DateOfSarha { get; set; }
        public int id { get; set; }
        public string OldHalekPrices { get; set; }
        public string NHalekPrices { get; set; }
       
    }
}
