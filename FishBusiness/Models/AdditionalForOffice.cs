using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.Models
{
    public class AdditionalForOffice
    {
        [Key]
        public int AdditionalForOfficeID { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }

        [ForeignKey("Person")]
        public int PersonID { get; set; }
        public DateTime Date { get; set; }

        public virtual Person Person { get; set; }
    }
}
