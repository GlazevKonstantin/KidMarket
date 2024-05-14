using System.ComponentModel.DataAnnotations;

namespace KidMarket
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public string UserName { get; set; }
        public string UserSurName { get; set; }
        public string UserLastName { get; set; }
        public string UserEmail{ get; set; }
        public string ProductName{ get; set; }
    }
}
