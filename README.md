# HeidelbergCement - .NET/C# Case Study

Dear applicant, thank you for your interest in a position at HeidelbergCement!

We want to make the best use of your and our time and give you a sense of the kind of work that is currently done in
HProduce to support our cement plants' operational excellence.

The following case study describes an interaction of a microservice and its corresponding business requirements. It is
your task to understand the requirements, how they are not currently being met, and to create a fix to the issue.

We would like to see your approach to the code base, your knowledge of microservices, concurrency issues, domain driven
design. Later on we would like you to present your solution to help us get a better sense of your skills and the way you
solve problems.

Please complete the case study and publish it one github, bitbucket, gitlab, etc. and send us the link to the repo.

> **IMPORTANT**: Please make an initial commit of the project to make your adaptions visible.

If you have any questions or run into problems, please send an email to buzurgmehr.arjmandi@heidelbergcement.com (cc
sebastian.walter@heidelbergcement.com).

# The Case Study

## Business Case

We have machines that run at cement plants that produce different kinds of cement. These machines are scheduled to run
at different times based on a variety of factors, including how much cement is required and what the energy costs for
the time are.

A **schedule** is defined as a collection of scheduled machine run times for a plant. Schedule items can never overlap.
If there are overlapping schedule items then the entire schedule is invalid.

Here is an example schedule in the form of a gantt chart where the machine run times are organized by the asset they
create. Imagine every `:` is a 15 minute interval

```
[::CEM-I:::]  [::::::::::::::CEM-II::::::::::::::::][:CEM-III:]
```

In json format this would look like the following:

```json
{
  "scheduleId": 12132891,
  "plantId": 1213,
  "updatedOn": "2021-12-01T12:44:17Z",
  "scheduleItems": [
    {
      "scheduleItemId": 1,
      "cementType": "CEM-I",
      "start": "2021-11-23T00:00:00Z",
      "end": "2021-11-23T02:15:00Z",
      "updatedOn": "2021-12-01T11:43:17Z"
    },
    {
      "scheduleItemId": 2,
      "cementType": "CEM-II",
      "start": "2021-11-23T03:00:00Z",
      "end": "2021-11-23T10:30:00Z",
      "updatedOn": "2021-12-01T11:43:17Z"
    },
    {
      "scheduleItemId": 3,
      "cementType": "CEM-III",
      "start": "2021-11-23T10:30:00Z",
      "end": "2021-11-23T11:00:00Z",
      "updatedOn": "2021-12-01T11:43:17Z"
    }
  ]
}
```

## The Problem

This project is a .NET 6 microservice that manages the schedules for the different plants.

It has the following end points

* GET   `schedule` allows getting the latest created schedule for a plant
* POST  `schedule` allows adding a schedule for a plant
* POST  `schedule/items` allows adding an item to a schedule.

Currently if you add an item then the service validates that the item does not conflict. Unfortunately if you make two
requests to add an item in quick succession we have a concurrency issue where both items are saved to the database,
immediately making the entire schedule invalid.

## Deliverables

There are two integration tests that have been created in
the `HeidelbergCement.CaseStudies.Concurrency.Api.Test.Integration/ItemsTests.cs` project/folder. These tests
demonstrate actual scenarios where the concurrency issues are present.

1. In the file `ANALYSIS.md`, put your analysis of the problem, its causes, possible solutions and your selected
   solution strategy.
2. The project should be modified so that the tests successfully pass (i.e. no more concurrency issues) and the business
   requirements are still fulfilled.
3. In the file `DDD.md`, put a short summary of DDD, its merits and possible drawbacks in the current solution.

# Project Setup Basics

Your purpose is to get all of the tests working.

You will find the following projects in the solution

* **Api** - The actual microservice scaffolding.
* **Api.Tests.Integration** - Integration tests related to the Api
* **Infrastructure** - Dealing with the database and other infra-related aspects.
* **Domain** - All of the business models and business logic relating to these models are contained here.
* **Common** - Any code which could be shared across all other projects (i.e. common extension methods or the like)

# Getting Started

1. Install .NET 6
1. Clone the repo.
1. Run the postgres service in docker compose `docker-compose up`
1. Run the tests (`dotnet test`)

