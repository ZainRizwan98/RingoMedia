
using RingoMedia.Data.Repositories;
using RingoMedia.Data.Repositories.Interfaces;
using RingoMedia.Models.Domain;
using RingoMedia.Models.Enums;
using RingoMedia.Models.Requests;

namespace RingoMedia.Services
{
    public class EmailReminderService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly EmailSender _emailSender;
        private readonly ILogger<EmailReminderService> _logger;

        public EmailReminderService(IServiceProvider serviceProvider, ILogger<EmailReminderService> logger, EmailSender emailSender)
        {
            _serviceProvider = serviceProvider;
            _emailSender = emailSender;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await SendDueReminders();
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // Run every minute
            }
        }

        private async Task SendDueReminders()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var reminderRepository = scope.ServiceProvider.GetRequiredService<IReminderRepository>();
                var dueReminders = await reminderRepository.FindAsync(new ReminderRequest
                {
                    Status = Models.Enums.ReminderStatus.Pending,
                    ValidTo = DateTime.UtcNow
                });

                foreach (var reminder in dueReminders)
                {
                    ReminderStatus status = ReminderStatus.Success;
                    try
                    {
                        await SendEmailAsync(reminder);
                    }
                    catch (Exception)
                    {
                        status = ReminderStatus.Failed;
                    }

                    await reminderRepository.UpdateAsync(reminder.Id, status);
                }
            }
        }

        private async Task SendEmailAsync(Reminder reminder)
        {
            await _emailSender.SendEmailAsync(reminder.Email, reminder.Title, reminder.Message);

            _logger.LogInformation($"Sending email reminder for: {reminder.Title} to {reminder.Email}");
        }
    }
}
