using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.Models
{
    public class Fisherman
    {
        [Key]
        public int FishermenID { get; set; }
        
        [Required(ErrorMessage ="برجاء ادخال اسم النفر")]
        [Display(Name ="اسم النفر")]
        public string FishermenName { get; set; }
        [ForeignKey("Sarha")]
        public int SarhaID { get; set; }
        public virtual Sarha Sarha { get; set; }

    }
}
