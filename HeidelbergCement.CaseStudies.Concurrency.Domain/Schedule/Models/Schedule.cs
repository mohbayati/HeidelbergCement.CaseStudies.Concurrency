using HeidelbergCement.CaseStudies.Concurrency.Domain.Schedule.Validation;

namespace HeidelbergCement.CaseStudies.Concurrency.Domain.Schedule.Models;

public class Schedule
{
    public Schedule()
    {
        ScheduleItems = new List<ScheduleItem>();
    }

    public Schedule(int plantCode, DateTime now)
    {
        PlantCode = plantCode;
        UpdatedOn = now;
        ScheduleItems = new List<ScheduleItem>();
    }
    public int ScheduleId { get; set; }
    public int PlantCode { get; set; }
    public DateTime UpdatedOn { get; set; }
    public ICollection<ScheduleItem> ScheduleItems { get; set; }

    public void AddItem(DateTime start, DateTime end, string cementType, DateTime now)
    {
        var item = new ScheduleItem(start, end, cementType, now);
        item.ValidateDoesNotOverlapWithItems(ScheduleItems.ToList());
        ScheduleItems.Add(item);
    }

    public void UpdateItem(int itemId, DateTime start, DateTime end, string cementType, DateTime now)
    {
        ScheduleItems = ScheduleItems
            .Select(it =>
            {
                if (it.ScheduleItemId == itemId)
                {
                    it.Update(start, end, cementType, now);
                }
                return it;
            }).ToList();
        UpdatedOn = now;
    }
}