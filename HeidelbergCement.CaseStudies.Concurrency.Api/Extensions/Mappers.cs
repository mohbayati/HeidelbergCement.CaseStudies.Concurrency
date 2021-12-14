using HeidelbergCement.CaseStudies.Concurrency.Domain.Schedule.Models;
using HeidelbergCement.CaseStudies.Concurrency.Dto.Response;

namespace HeidelbergCement.CaseStudies.Concurrency.Extensions;

public static class Mappers
{
    public static ScheduleItemResponseDto MapToScheduleItemDto(this ScheduleItem scheduleItem)
    {
        return new ScheduleItemResponseDto
        {
            End = scheduleItem.End,
            Start = scheduleItem.Start,
            CementType = scheduleItem.CementType,
            ScheduleId = scheduleItem.ScheduleId,
            UpdatedOn = scheduleItem.UpdatedOn,
            NumberOfTimesUpdated = scheduleItem.NumberOfTimesUpdated,
            ScheduleItemId = scheduleItem.ScheduleItemId
        };
    }
    public static ScheduleResponseDto MapToScheduleDto(this Schedule schedule)
    {
        return new ScheduleResponseDto
        {
            PlantCode = schedule.PlantCode,
            ScheduleId = schedule.ScheduleId,
            UpdatedOn = schedule.UpdatedOn,
            ScheduleItems = schedule.ScheduleItems.Select(MapToScheduleItemDto).ToList()
        };
    }
}