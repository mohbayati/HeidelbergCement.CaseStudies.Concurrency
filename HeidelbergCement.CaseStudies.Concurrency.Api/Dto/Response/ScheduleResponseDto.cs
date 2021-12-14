namespace HeidelbergCement.CaseStudies.Concurrency.Dto.Response;

public class ScheduleResponseDto
{
    public int ScheduleId { get; set; }
    public int PlantCode { get; set; }
    public DateTime UpdatedOn { get; set; }
    public ICollection<ScheduleItemResponseDto> ScheduleItems { get; set; }
}