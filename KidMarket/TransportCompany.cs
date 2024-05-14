using System;
using System.ComponentModel.DataAnnotations;

namespace KidMarket
{
    public class TransportCompany
    {
        [Key]
        public int TransportCompanyID { get; set; }
        public string TransportCompanyName { get; set; }
        public string DeliveryTime { get; set; }
        public string TermsOfDelivery { get; set; }
    }
}