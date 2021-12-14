using System.Net.Mime;
using HeidelbergCement.CaseStudies.Concurrency.Dto.Input;
using HeidelbergCement.CaseStudies.Concurrency.Dto.Response;
using HeidelbergCement.CaseStudies.Concurrency.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HeidelbergCement.CaseStudies.Concurrency.Controllers;

[Route("schedule")]
[ApiController]
public class ScheduleController: ControllerBase
{
    private readonly IScheduleService _scheduleService;

    public ScheduleController(IScheduleService scheduleService)
    {
        _scheduleService = scheduleService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ScheduleResponseDto), StatusCodes.Status200OK)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<ActionResult<ScheduleResponseDto>> GetDraftSchedule(int plantCode)
    {
        var schedule = await _scheduleService.GetLatestScheduleForPlant(plantCode);
        return Ok(schedule);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ScheduleResponseDto), StatusCodes.Status200OK)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<ActionResult<ScheduleResponseDto>> PostSchedule(int plantCode, List<ScheduleInputItemDto> scheduleInputItems)
    {
        var schedule = await _scheduleService.AddNewSchedule(plantCode, scheduleInputItems);
        return Ok(schedule);
    }

    
    [HttpPost("items")]
    [ProducesResponseType(typeof(ScheduleResponseDto), StatusCodes.Status200OK)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<ActionResult<ScheduleItemResponseDto>> PostScheduleItem(int scheduleId, ScheduleInputItemDto scheduleInputItem)
    {
        var scheduleItem = await _scheduleService.AddItemToSchedule(scheduleId, scheduleInputItem);
        return Ok(scheduleItem);
    }
    
    [HttpPut("items/{itemId}")]
    [ProducesResponseType(typeof(ScheduleResponseDto), StatusCodes.Status200OK)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<ActionResult<ScheduleItemResponseDto>> PutScheduleItem(int scheduleId, int itemId, ScheduleInputItemDto scheduleInputItem)
    {
        var scheduleItem = await _scheduleService.ChangeScheduleItem(scheduleId, itemId, scheduleInputItem);
        return Ok(scheduleItem);
    }
}