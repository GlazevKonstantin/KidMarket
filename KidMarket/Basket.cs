using System.ComponentModel.DataAnnotations;

namespace KidMarket
{
    public class Basket
    {
        [Key]
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int UserID { get; set; }
    }
}