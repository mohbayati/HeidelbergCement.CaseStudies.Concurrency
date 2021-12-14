using HeidelbergCement.CaseStudies.Concurrency.Domain.Schedule.Models;
using Microsoft.EntityFrameworkCore;

namespace HeidelbergCement.CaseStudies.Concurrency.Infrastructure.DbContexts.Schedule;

public interface IScheduleDbContext: IDbContext
{
    DbSet<Domain.Schedule.Models.Schedule> Schedules { get; set; }
        
    DbSet<ScheduleItem> ScheduleItems { get; set; }
}