using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FishBusiness.ViewModels
{
    public class OfficeVM
    {
        #region income
        // العمولات
        public decimal Commisions { get; set; }
        // اجمالي الفواتير اللي سافرت ورجعت
        public decimal IsellerReceiptsTotal { get; set; }
        // فواتير خارجيه للمراكب للشريكة
        public decimal externalReceiptsTotal { get; set; }
        // ايرادات المراكب الشريكة غير الخارجيه
        public decimal SharedBoatsReceiptsTotal { get; set; }
        // المحصل
        public decimal collectorForUsTotal { get; set; }
        //   المسحوبات بتاع ريس المركب
        public decimal LeaderLoansPaybackTotal { get; set; }
        // اجمالي مبيعات
        public decimal SalesTotal { get; set; }
        #endregion

        #region outcome
        // احمد فنح الله
        public decimal FathallahTotal { get; set; }
        // اخدها مننا
        public decimal CollectorTotalFromUs { get; set; }
        //  طلعها للتجار والمراكب وفتح الله
        public decimal CollectorTotalforMerchantsAndHalek { get; set; }
        //  المشتريات من ضمنها التلج والعمال والعربيات
        public decimal BuyingTotal { get; set; } 
        #endregion
    }
}
