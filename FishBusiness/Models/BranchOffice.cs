using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.Models
{
    public class BranchOffice
    {
        public int ID { get; set; }
        //رصيد مرحل
        public decimal CurrentCredit { get; set; }
        // اجمالي المحصل
        public decimal Collecting { get; set; }
        // تمويل الحاج مجدي
        public decimal OfficeMoney { get; set; }
        // اجمالي المصروف اليومي
        public decimal ExpensesTotal { get; set; }
        // اجمالي الايرادات
        public decimal IncomeTotal { get; set; }
        // اجمالي قبض السرح
        public decimal SarhasTotal { get; set; }
        // شهرية فتح الله
        public decimal FathallahSalary { get; set; }
        // اجمالي قبض السواقين
        public decimal DriversSalary { get; set; }

        public DateTime Date { get; set; }

    }
}
