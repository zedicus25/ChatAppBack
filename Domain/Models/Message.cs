
namespace Domain.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string MessageText { get; set; }
        public int FromContactId { get; set; }
        public int ToContactId { get; set; }
        public DateTime SentTime { get; set; }
    }
}
