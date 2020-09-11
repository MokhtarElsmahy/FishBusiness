using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.ViewModels
{
    public class BoatInfoVM
    {
        public int BoatID { get; set; }
        [Required(ErrorMessage = "ادخل اسم المركب")]
        [Display(Name = "اسم المركب")]
        public string BoatName { get; set; }

        [Required(ErrorMessage = "ادخل اسم ريس المركب")]
        [Display(Name = "ريس المركب")]
        public string BoatLeader { get; set; }

        public decimal TotalOfExpenses { get; set; }

        [Display(Name = "ديون الهالك")]
        public decimal DebtsOfHalek { get; set; }
        public decimal IncomeOfSharedBoat { get; set; }

        [Display(Name = "ديون اعطال")]
        public decimal DebtsOfMulfunction { get; set; }

        [Display(Name = "ديون الشغل")]
        public decimal DebtsOfStartingWork { get; set; }

        [Required(ErrorMessage = "ادخل رخصة المركب")]
        [Display(Name = "رخصة المركب")]
        public string BoatLicenseNumber { get; set; }


        public decimal DebtsOfLeader { get; set;}
            [Required(ErrorMessage = "ادخل رقم المركب")]
            [Display(Name = " رقم المركب")]
            public string BoatNumber { get; set; }

        [Display(Name = " صورة المركب")]
        public string BoatImage { get; set; }
        public string Type { get; set; }
        public int TypeID { get; set; }


    }
}
