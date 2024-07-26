using RingoMedia.Models.Domain;
using RingoMedia.Models.Enums;
using RingoMedia.Models.Requests;

namespace RingoMedia.Data.Repositories.Interfaces
{
    public interface IReminderRepository
    {
        Task<IEnumerable<Reminder>> FindAsync(ReminderRequest? request);
        Task<Reminder> GetByIdAsync(int id);
        Task AddAsync(Reminder reminder);
        Task UpdateAsync(int id, ReminderStatus status);
        Task RemoveAsync(int id);
    }
}
