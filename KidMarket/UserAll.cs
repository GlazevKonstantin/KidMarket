using System.ComponentModel.DataAnnotations;

namespace KidMarket
{
    public class UserAll
    {
        [Key]
        public int ID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}