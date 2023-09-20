using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Persistence
{
    public class DevEventsDbContext : DbContext
    {
        public DevEventsDbContext(DbContextOptions<DevEventsDbContext> options) : base(options)
        {
            
        }
        public DbSet<DevEvent>DevEvents {  get; set; }

        public DbSet<DevEventSpeaker> DevEventSpeaker { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DevEvent>(e =>
            {
                e.HasKey(de => de.id);

                e.Property(de => de.Title).IsRequired(false);

                e.Property(de => de.Description).HasMaxLength(200).HasColumnType("varchar(200)");

                e.Property(de => de.StartDate).HasColumnName("start_date");

                e.Property(de => de.EndDate).HasColumnName("end_date");

                e.HasMany(de => de.Speakers).WithOne().HasForeignKey(s => s.DevEventId);
            });

            builder.Entity<DevEventSpeaker>(e =>
            {
                e.HasKey(de => de.Id);
            });
        }

    }
}
