using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using HeidelbergCement.CaseStudies.Concurrency.Api.Test.Infrastructure;
using HeidelbergCement.CaseStudies.Concurrency.Dto.Input;
using HeidelbergCement.CaseStudies.Concurrency.Dto.Response;
using Xunit;
using Xunit.Abstractions;

namespace HeidelbergCement.CaseStudies.Concurrency.Api.Test;

public class ItemsTests : IntegrationTestBase
{
    public ItemsTests(IntegrationTestFixture integrationTestFixture, ITestOutputHelper output) : base(integrationTestFixture, output)
    {
    }

    [Fact]
    public async Task GivenScheduleWithNoItems_WhenTwoSimultaneousIdenticalAddItemRequests_ThenOneItemIsAddedAndTheOtherRejected()
    {
        //Setup
        var fixture = new Fixture();
        var plantCode = fixture.Create<int>().ToString();
        var itemToAdd = new ScheduleInputItemDto
        {
            Start = DateTime.UtcNow,
            End = DateTime.UtcNow.AddHours(1),
            CementType = "CEM-I"
        };

        var addScheduleRequest = NewRequest
            .AddRoute("schedule")
            .AddQueryParams("plantCode", plantCode);
        var latestScheduleRequest = NewRequest
            .AddRoute("schedule")
            .AddQueryParams("plantCode", plantCode);
        var addItemForScheduleRequest = (string scheduleId) => NewRequest
            .AddRoute("schedule/Items")
            .AddQueryParams("scheduleId", scheduleId);

        // Exercise
        await addScheduleRequest.Post(new ScheduleInputItemDto[] { });

        // First let's get the schedule before adding any items. This schedule is currently empty..
        var scheduleBeforeAddition = await latestScheduleRequest.Get<ScheduleResponseDto>();

        var scheduleId = scheduleBeforeAddition.ScheduleId.ToString();
        var addItemRequest = addItemForScheduleRequest(scheduleId);

        // Simultaneously start two tasks that will make the same exact item addition request.
        // This is a race condition, the first request should pass and the second should fail.
        var itemAddResponses = await Task.WhenAll(addItemRequest.Post(itemToAdd, false), addItemRequest.Post(itemToAdd, false));

        //Finally let's get the schedule after the item addition requests. It should have only one item in it.
        var scheduleAfterAddition = await latestScheduleRequest.Get<ScheduleResponseDto>();

        // Verify
        scheduleBeforeAddition.ScheduleItems.Count.Should().Be(0);
        //TEST FAILS HERE - only one of the items should be added and the second should cause a conflict
        scheduleAfterAddition.ScheduleItems.Count.Should().Be(1);

        var failures = itemAddResponses.ToList().Where(it => it.IsSuccessStatusCode == false);
        var successes = itemAddResponses.ToList().Where(it => it.IsSuccessStatusCode == true);

        failures.Count().Should().Be(1);
        successes.Count().Should().Be(1);
    }

    [Fact]
    public async Task GivenScheduleWithItem_WhenTwoClientsAreChangingTheSingleItem_ThenItemModificationShouldHappenInSequence()
    {
        //Setup
        var fixture = new Fixture();
        var plantCode = fixture.Create<int>().ToString();
        var itemDto = new ScheduleInputItemDto
        {
            Start = DateTime.UtcNow,
            End = DateTime.UtcNow.AddHours(1),
            CementType = "CEM-I"
        };

        var addScheduleRequest = NewRequest
            .AddRoute("schedule")
            .AddQueryParams("plantCode", plantCode);
        var latestScheduleRequest = NewRequest
            .AddRoute("schedule")
            .AddQueryParams("plantCode", plantCode);
        var changeItemForScheduleRequest = (string scheduleId, string itemId) => NewRequest
            .AddRoute($"schedule/Items/{itemId}")
            .AddQueryParams("scheduleId", scheduleId);

        //Exercise
        //Make new schedule
        await addScheduleRequest.Post(new List<ScheduleInputItemDto> { itemDto });

        var scheduleBeforeChanges = await latestScheduleRequest.Get<ScheduleResponseDto>();
        var scheduleId = scheduleBeforeChanges.ScheduleId.ToString();
        var existingItemId = scheduleBeforeChanges.ScheduleItems.First().ScheduleItemId;

        var itemChangeRequest = changeItemForScheduleRequest(scheduleId, existingItemId.ToString());

        // Send two simultaneous item change requests
        var itemChangeResponses = await Task.WhenAll(itemChangeRequest.Put(itemDto,false), itemChangeRequest.Put(itemDto,false));

        //Get the schedule after item change requests, should have only one item and the item should have an update counter of only 1
        var scheduleAfterChanges = await latestScheduleRequest.Get<ScheduleResponseDto>();

        //verify
        scheduleBeforeChanges.ScheduleItems.Count.Should().Be(1);
        scheduleBeforeChanges.ScheduleItems.First().NumberOfTimesUpdated.Should().Be(0);

        scheduleAfterChanges.ScheduleItems.Count.Should().Be(1);
        scheduleAfterChanges.ScheduleItems.First().NumberOfTimesUpdated.Should().Be(1);

        var failures = itemChangeResponses.ToList().Where(it => it.IsSuccessStatusCode == false);
        var successes = itemChangeResponses.ToList().Where(it => it.IsSuccessStatusCode == true);

        //TEST FAILS HERE, as one of the calls should fail
        failures.Count().Should().Be(1);
        successes.Count().Should().Be(1);
    }
}