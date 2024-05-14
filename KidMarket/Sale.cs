using System;
using System.ComponentModel.DataAnnotations;

namespace KidMarket
{
    public class Sale
    {
        [Key]
        public int SaleID { get; set; }
        public string DiscountTerms { get; set; }
        public string DiscountAmount { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}