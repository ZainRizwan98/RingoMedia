using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RingoMedia.Data.Repositories.Interfaces;
using RingoMedia.Exceptions;
using RingoMedia.Models.Domain;
using RingoMedia.Models.Entities;
using RingoMedia.Models.Enums;
using RingoMedia.Models.Requests;
using RingoMedia.Models.ViewModels;

namespace RingoMedia.Data.Repositories
{
    public class ReminderRepository : IReminderRepository
    {
        private readonly RingoMediaDbContext _context;
        private readonly IMapper _mapper;

        public ReminderRepository(RingoMediaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Reminder>> FindAsync(ReminderRequest? request)
        {
            IEnumerable<ReminderEntity> reminders = await _context.Reminders
                .Where(x => 
                    request == null ||
                    (!request.Status.HasValue || x.Status == request.Status.Value) &&
                    (!request.ValidTo.HasValue || x.DateTime <= request.ValidTo.Value)
            ).ToListAsync();

            return _mapper.Map<IEnumerable<Reminder>>(reminders);
        }

        public async Task<Reminder> GetByIdAsync(int id)
        {
            ReminderEntity? reminder = await _context.Reminders.FindAsync(id);

            if (reminder == null)
            {
                throw new NotFoundException($"Reminder with id {id} not found.");
            }

            return _mapper.Map<Reminder>(reminder);
        }

        public async Task AddAsync(Reminder reminder)
        {
            await _context.Reminders.AddAsync(new ReminderEntity
            {
                Title = reminder.Title,
                Message = reminder.Message,
                Email = reminder.Email,
                DateTime = reminder.DateTime.UtcDateTime,
                Status = ReminderStatus.Pending
            });
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, ReminderStatus status)
        {
            ReminderEntity? reminder = await _context.Reminders.FindAsync(id);

            if(reminder == null)
            {
                throw new NotFoundException($"Product with id {id} not found.");
            }

            reminder.Status = status;
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(int id)
        {
            ReminderEntity? reminder = await _context.Reminders.FindAsync(id);

            if (reminder == null)
            {
                throw new NotFoundException($"Product with id {id} not found.");
            }

            _context.Reminders.Remove(reminder);
            await _context.SaveChangesAsync();
        }
    }
}
