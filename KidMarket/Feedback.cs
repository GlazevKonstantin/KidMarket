using System.ComponentModel.DataAnnotations;

namespace KidMarket
{
    public class Feedback
    {
        [Key]
        public int FeedbackID { get; set; }
        public string Review { get; set; }
    }
}