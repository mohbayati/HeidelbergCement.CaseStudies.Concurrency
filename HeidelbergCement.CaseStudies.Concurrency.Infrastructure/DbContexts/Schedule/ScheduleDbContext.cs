using HeidelbergCement.CaseStudies.Concurrency.Domain.Schedule.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HeidelbergCement.CaseStudies.Concurrency.Infrastructure.DbContexts.Schedule;

public class ScheduleDbContext: DbContext, IScheduleDbContext
{
    public ScheduleDbContext(DbContextOptions<ScheduleDbContext> options) : base(options)
    {
    }
    public DbSet<Domain.Schedule.Models.Schedule> Schedules { get; set; }
    public DbSet<ScheduleItem> ScheduleItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        SetupDateTimeConversion(modelBuilder);
        
        modelBuilder.Entity<ScheduleItem>().HasKey(it => it.ScheduleItemId);
        modelBuilder.Entity<ScheduleItem>().Property(x => x.ScheduleId).ValueGeneratedNever();
        modelBuilder.Entity<ScheduleItem>().Property(it => it.Start).IsRequired();
        modelBuilder.Entity<ScheduleItem>().Property(it => it.End).IsRequired();
        modelBuilder.Entity<ScheduleItem>().Property(it => it.NumberOfTimesUpdated);
        modelBuilder.Entity<ScheduleItem>().Property(it => it.CementType).IsRequired();
        modelBuilder.Entity<ScheduleItem>().Property(it => it.UpdatedOn).IsRequired();

        modelBuilder.Entity<ScheduleItem>()
            .HasOne(x => x.Schedule)
            .WithMany(bl => bl.ScheduleItems)
            .HasForeignKey(x => x.ScheduleId);
        
        modelBuilder.Entity<Domain.Schedule.Models.Schedule>().HasKey(it => it.ScheduleId);
        modelBuilder.Entity<Domain.Schedule.Models.Schedule>().Property(it => it.PlantCode).IsRequired();
        modelBuilder.Entity<Domain.Schedule.Models.Schedule>().Property(it => it.UpdatedOn).IsRequired();
    }
    public void SetModified(object entity)
    {
        Entry(entity).State = EntityState.Modified;
    }
    private void SetupDateTimeConversion(ModelBuilder modelBuilder)
    {
        var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
            v => v.ToUniversalTime(),
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        var nullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
            v => v.HasValue ? v.Value.ToUniversalTime() : v,
            v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (entityType.IsKeyless)
            {
                continue;
            }

            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTime))
                {
                    property.SetValueConverter(dateTimeConverter);
                }
                else if (property.ClrType == typeof(DateTime?))
                {
                    property.SetValueConverter(nullableDateTimeConverter);
                }
            }
        }
    }
}