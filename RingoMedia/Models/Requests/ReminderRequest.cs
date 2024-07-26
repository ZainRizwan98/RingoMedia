using RingoMedia.Models.Enums;

namespace RingoMedia.Models.Requests
{
    public class ReminderRequest
    {
        public ReminderStatus? Status { get; set; }
        public DateTime? ValidTo { get; set; }
    }
}
