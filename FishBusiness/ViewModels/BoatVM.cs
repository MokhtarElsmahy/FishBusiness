using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.ViewModels
{
    public class BoatVM
    {
        public int BoatID { get; set; }
        [Required(ErrorMessage = "ادخل اسم المركب")]
        [Display(Name = "اسم المركب")]
        public string BoatName { get; set; }

        [Required(ErrorMessage = "ادخل اسم ريس المركب")]
        [Display(Name = "ريس المركب")]
        public string BoatLeader { get; set; }



        [Display(Name = "ديون الهالك")]
        public decimal DebtsOfHalek { get; set; }

        [Display(Name = "ديون اعطال")]
        public decimal DebtsOfMulfunction { get; set; }

        [Display(Name = "ديون الشغل")]
        public decimal DebtsOfStartingWork { get; set; }

        [Required(ErrorMessage = "ادخل رخصة المركب")]
        [Display(Name = "رخصة المركب")]
        public string BoatLicenseNumber { get; set; }

        [Required(ErrorMessage = "ادخل رقم المركب")]
        [Display(Name = " رقم المركب")]
        public string BoatNumber { get; set; }

        [Display(Name = " صورة المركب")]
        public string BoatImage { get; set; }
        public int TypeID { get; set; }

        public IFormFile File { get; set; }
    }
}
