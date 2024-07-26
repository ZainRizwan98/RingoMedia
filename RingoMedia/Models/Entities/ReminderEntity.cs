using RingoMedia.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace RingoMedia.Models.Entities
{
    public class ReminderEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Message { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [Required]
        [StringLength(200)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public ReminderStatus Status { get; set; }
    }
}
