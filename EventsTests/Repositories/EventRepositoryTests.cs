using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Data.Context;
using Data.Repositories;
using Data.Entities;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventsTests.Repositories;

public class EventRepositoryTests
{
    private readonly EventDbContext _context;
    private readonly EventRepository _repository;

    public EventRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<EventDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new EventDbContext(options);
        _repository = new EventRepository(_context);
    }

    [Fact]
    public async Task AddAsync_Should_Save_Event_To_Database()
    {
        // Arrange
        var newEvent = new EventEntity
        {
            Id = Guid.NewGuid(),
            EventName = "Test Event",
            Category = "Test",
            Location = "Teststad",
            Description = "Beskrivning",
            Price = 99,
            StartDateTime = DateTime.UtcNow,
            Status = Domain.Models.EventStatus.Active
        };

        // Act
        var result = await _repository.AddAsync(newEvent);

        // Assert
        result.Succeeded.Should().BeTrue();
        var exists = await _context.Events.FindAsync(newEvent.Id);
        exists.Should().NotBeNull();
        exists!.EventName.Should().Be("Test Event");
    }
    [Fact]
    public async Task GetEntityWithDetailsAsync_Should_Return_Event_With_Details()
    {
        // Arrange
        var eventId = Guid.NewGuid();

        var eventEntity = new EventEntity
        {
            Id = eventId,
            EventName = "Test Event",
            Category = "Music",
            Location = "Venue",
            Description = "Testdesc",
            Price = 100,
            StartDateTime = DateTime.UtcNow,
            Status = Domain.Models.EventStatus.Active,
            Merchandise = new List<MerchandiseEntity>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "T-shirt",
                Price = 25,
                EventId = eventId
            }
        },
            Packages = new List<PackageEntity>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "VIP",
                Price = 150,
                EventId = eventId,
                PackageDetails = "Access to VIP Lounge"
            }
        },
            SeatPlan = new SeatPlanEntity
            {
                Id = Guid.NewGuid(),
                EventId = eventId,
                Categories = new List<SeatCategoryEntity>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Front Row",
                    Price = 300,
                    AvailableSeats = 10
                }
            }
            }
        };

        await _context.Events.AddAsync(eventEntity);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetEntityWithDetailsAsync(eventId);
        // Assert
        result.Succeeded.Should().BeTrue();
        result.Result.Should().NotBeNull();
        result.Result!.Merchandise.Should().HaveCount(1);
        result.Result.Packages.Should().HaveCount(1);
        result.Result.Packages.First().PackageDetails.Should().Be("Access to VIP Lounge");
        result.Result.SeatPlan.Should().NotBeNull();
        result.Result.SeatPlan!.Categories.Should().HaveCount(1);
    }
}