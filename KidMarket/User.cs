using System.ComponentModel.DataAnnotations;
namespace KidMarket
{
    public class User
    {
        [Key] 
        public int UserID { get; set; }
        public string UserSurname { get; set; }
        public string UserName { get; set; }
        public string UserLastName { get; set; }
        public string UserPhoneNumber  {get; set; }
        public string UserEmail { get; set; }
        public string UserLogin { get; set; }
        public string UserPassword { get; set; }
        public byte[] ImageData { get; set; }
    }
}