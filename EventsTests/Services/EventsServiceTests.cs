using Data.Context;
using Data.Models;
using Data.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Threading.Tasks;
using System;
using Xunit;
using Data.Entities;
using Business.Services;
using FluentAssertions;

namespace EventsTests.Services;

public class EventsServiceTests
{
    private readonly Mock<IEventRepository> _repoMock;
    private readonly EventsService _service;

    public EventsServiceTests()
    {
        _repoMock = new Mock<IEventRepository>();

        var options = new DbContextOptionsBuilder<EventDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new EventDbContext(options);
        _service = new EventsService(_repoMock.Object, context);
    }

    [Fact]
    public async Task CreateEventAsync_Should_Return_Success_When_Model_Is_Valid()
    {
        // Arrange
        var model = new CreateEventFormData
        {
            EventName = "Test Event",
            Category = "Music",
            Location = "Venue",
            Description = "Great event",
            Price = 150,
            StartDateTime = DateTime.UtcNow.AddDays(1)
        };

        _repoMock.Setup(r => r.AddAsync(It.IsAny<EventEntity>()))
            .ReturnsAsync(new RepositoryResult<bool>
            {
                Succeeded = true,
                StatusCode = 201
            });

        // Act
        var result = await _service.CreateEventAsync(model);

        // Assert
        result.Succeeded.Should().BeTrue();
        result.StatusCode.Should().Be(201);
        result.Error.Should().BeNull();
    }

    [Fact]
    public async Task CreateEventAsync_Should_Return_Error_When_Model_Is_Null()
    {
        // Act
        var result = await _service.CreateEventAsync(null!);
        // Assert
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(400);
        result.Error.Should().Be("Input was null.");
    }

    [Fact]
    public async Task GetEventByIdAsync_Should_Return_Event_When_It_Exists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var expectedEvent = new Event
        {
            Id = id,
            EventName = "Test Event",
            Category = "Music",
            Location = "Venue",
            Description = "Nice",
            Price = 100,
            StartDateTime = DateTime.UtcNow,
            Status = EventStatus.Active
        };

        _repoMock.Setup(r => r.GetAsync(e => e.Id == id, It.IsAny<System.Linq.Expressions.Expression<Func<EventEntity, object>>[]>()))
            .ReturnsAsync(new RepositoryResult<Event>
            {
                Succeeded = true,
                StatusCode = 200,
                Result = expectedEvent
            });

        // Act
        var result = await _service.GetEventByIdAsync(id);

        // Assert
        result.Succeeded.Should().BeTrue();
        result.StatusCode.Should().Be(200);
        result.Result.Should().NotBeNull();
        result.Result!.EventName.Should().Be("Test Event");
    }
    [Fact]
    public async Task GetEventByIdAsync_Should_Return_404_When_Event_Not_Found()
    {
        // Arrange
        var id = Guid.NewGuid();

        _repoMock.Setup(r => r.GetAsync(e => e.Id == id, It.IsAny<System.Linq.Expressions.Expression<Func<EventEntity, object>>[]>()))
            .ReturnsAsync(new RepositoryResult<Event>
            {
                Succeeded = false,
                StatusCode = 404,
                Error = "Event not found."
            });

        // Act
        var result = await _service.GetEventByIdAsync(id);

        // Assert
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(404);
        result.Error.Should().Be("Event not found.");
        result.Result.Should().BeNull();
    }
    [Fact]
    public async Task UpdateEventAsync_Should_Update_Event_If_It_Exists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dbOptions = new DbContextOptionsBuilder<EventDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var existingEvent = new EventEntity
        {
            Id = id,
            EventName = "Old Name",
            Category = "Music",
            Location = "Old Location",
            Description = "Old Desc",
            Price = 50,
            StartDateTime = DateTime.UtcNow
        };

        await using (var context = new EventDbContext(dbOptions))
        {
            context.Events.Add(existingEvent);
            await context.SaveChangesAsync();
        }

        await using (var context = new EventDbContext(dbOptions))
        {
            var service = new EventsService(_repoMock.Object, context);

            var updatedEvent = new EventEntity
            {
                Id = id,
                EventName = "New Name",
                Category = "Updated",
                Location = "New Location",
                Description = "New Desc",
                Price = 100,
                StartDateTime = DateTime.UtcNow
            };

            // Act
            var result = await service.UpdateEventAsync(updatedEvent);

            // Assert
            result.Should().NotBeNull();
            result!.EventName.Should().Be("New Name");
            result.Location.Should().Be("New Location");
        }
    }
    [Fact]
    public async Task UpdateEventAsync_Should_Return_Null_When_Event_Does_Not_Exist()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dbOptions = new DbContextOptionsBuilder<EventDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        await using var context = new EventDbContext(dbOptions);
        var service = new EventsService(_repoMock.Object, context);

        var updatedEvent = new EventEntity
        {
            Id = id,
            EventName = "Doesn't exist",
            Category = "X",
            Location = "Y",
            Description = "Z",
            Price = 100,
            StartDateTime = DateTime.UtcNow
        };

        // Act
        var result = await service.UpdateEventAsync(updatedEvent);

        // Assert
        result.Should().BeNull();
    }
    [Fact]
    public async Task DeleteEventAsync_Should_Delete_Existing_Event()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dbOptions = new DbContextOptionsBuilder<EventDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var eventEntity = new EventEntity
        {
            Id = id,
            EventName = "To Delete",
            Category = "Test",
            Location = "Here",
            Description = "Will be deleted",
            Price = 10,
            StartDateTime = DateTime.UtcNow
        };

        await using (var context = new EventDbContext(dbOptions))
        {
            context.Events.Add(eventEntity);
            await context.SaveChangesAsync();
        }

        await using (var context = new EventDbContext(dbOptions))
        {
            var service = new EventsService(_repoMock.Object, context);

            // Act
            var result = await service.DeleteEventAsync(id);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.StatusCode.Should().Be(200);

            var check = await context.Events.FindAsync(id);
            check.Should().BeNull(); // Event should actually be deleted
        }
    }
    [Fact]
    public async Task DeleteEventAsync_Should_Return_404_When_Event_Does_Not_Exist()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dbOptions = new DbContextOptionsBuilder<EventDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var context = new EventDbContext(dbOptions);
        var service = new EventsService(_repoMock.Object, context);

        // Act
        var result = await service.DeleteEventAsync(id);

        // Assert
        result.Succeeded.Should().BeFalse();
        result.StatusCode.Should().Be(404);
        result.Error.Should().Be("Event not found.");
    }
}




