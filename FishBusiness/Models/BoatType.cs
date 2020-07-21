using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.Models
{
    public class BoatType
    {
        [Key]
        public int TypeID { get; set; }
        [Required(ErrorMessage ="برجاء ادخال نوع المركب")]
        public string TypeName { get; set; }

        public virtual ICollection<Boat> Boats { get; set; }
    }
}
