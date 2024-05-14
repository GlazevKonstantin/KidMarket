using System.ComponentModel.DataAnnotations;
namespace KidMarket
{
    public class Admin
    {
        [Key]
        public int AdminID { get; set; }
        public string AdminName { get; set; }
        public string AdminSurname { get; set; }
        public string AdminLastName { get; set; }
        public string AdminLogin { get; set; }
        public string AdminPassword { get; set; }
        public byte[] ImageData { get; set; }
    }
}
