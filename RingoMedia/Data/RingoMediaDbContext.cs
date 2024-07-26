using Microsoft.EntityFrameworkCore;
using RingoMedia.Models.Entities;
using RingoMedia.Models.Domain;

namespace RingoMedia.Data
{
    public class RingoMediaDbContext : DbContext
    {
        public RingoMediaDbContext(DbContextOptions<RingoMediaDbContext> options)
            : base(options) { }

        public DbSet<DepartmentEntity> Departments { get; set; }
        public DbSet<ReminderEntity> Reminders { get; set; }
    }
}
