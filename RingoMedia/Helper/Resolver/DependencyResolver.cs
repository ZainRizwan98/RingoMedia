using RingoMedia.Data.Repositories.Interfaces;
using RingoMedia.Data.Repositories;
using RingoMedia.Services;
using RingoMedia.Data;
using Microsoft.EntityFrameworkCore;

namespace RingoMedia.Helper.Resolver
{
    public static class DependencyResolver
    {
        public static void Resolve(this IServiceCollection services, ConfigurationManager config)
        {
            services.AddDbContext<RingoMediaDbContext>(options =>
            {
                //options.UseInMemoryDatabase("NewDb");
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IReminderRepository, ReminderRepository>();
            services.AddAutoMapper(typeof(Program));

            services.AddSingleton<EmailSender>(sp => new EmailSender(
                config["Email:SmtpHost"],
                int.Parse(config["Email:SmtpPort"]),
                config["Email:SmtpUser"],
                config["Email:SmtpPass"]
            ));

            services.AddHostedService<EmailReminderService>();
        }
    }
}
