using HeidelbergCement.CaseStudies.Concurrency.Domain.Schedule.Repositories;
using HeidelbergCement.CaseStudies.Concurrency.Infrastructure.DbContexts.Schedule;
using Microsoft.EntityFrameworkCore;

namespace HeidelbergCement.CaseStudies.Concurrency.Infrastructure.Repositories.Schedule;

public class ScheduleRepository: GenericRepository<Domain.Schedule.Models.Schedule>, IScheduleRepository
{
    public ScheduleRepository(IScheduleDbContext context) : base(context)
    {
    }

    public Task<Domain.Schedule.Models.Schedule> GetLastUpdatedScheduleForPlant(int plantCode)
    {
        return FindByInclude(it => it.PlantCode == plantCode, it => it.ScheduleItems)
            .OrderByDescending(it => it.UpdatedOn)
            .SingleAsync();
    }

    public Task<Domain.Schedule.Models.Schedule> GetScheduleById(int scheduleId)
    {
        return FindByInclude(it => it.ScheduleId == scheduleId, it => it.ScheduleItems).SingleAsync();
    }
}