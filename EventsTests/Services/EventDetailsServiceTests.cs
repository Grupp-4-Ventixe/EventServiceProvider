using Xunit;
using Moq;
using FluentAssertions;
using Business.Services;
using Data.Repositories;
using Domain.Models;
using Data.Entities;
using Data.Models;
using System.Threading.Tasks;
using System;

namespace EventsTests.Services
{
    public class EventDetailsServiceTests
    {
        private readonly Mock<IEventRepository> _repoMock;
        private readonly EventDetailsService _service;

        public EventDetailsServiceTests()
        {
            _repoMock = new Mock<IEventRepository>();
            _service = new EventDetailsService(_repoMock.Object);
        }

        [Fact]
        public async Task GetEventDetailsByIdAsync_Should_Return_Details_When_Event_Exists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var entity = new EventEntity
            {
                Id = id,
                EventName = "Details Test",
                Category = "Category",
                StartDateTime = DateTime.UtcNow,
                Location = "Test Location",
                Description = "Test",
                Price = 123,
                Status = EventStatus.Active
            };

            _repoMock.Setup(r => r.GetEntityWithDetailsAsync(id))
                .ReturnsAsync(new RepositoryResult<EventEntity>
                {
                    Succeeded = true,
                    StatusCode = 200,
                    Result = entity
                });

            // Act
            var result = await _service.GetEventDetailsByIdAsync(id);

            // Assert
            result.Succeeded.Should().BeTrue();
            result.Result.Should().NotBeNull();
            result.Result!.EventName.Should().Be("Details Test");
        }

        [Fact]
        public async Task GetEventDetailsByIdAsync_Should_Return_404_When_Event_Not_Found()
        {
            // Arrange
            var id = Guid.NewGuid();

            _repoMock.Setup(r => r.GetEntityWithDetailsAsync(id))
                .ReturnsAsync(new RepositoryResult<EventEntity>
                {
                    Succeeded = false,
                    StatusCode = 404,
                    Error = "Event not found"
                });

            // Act
            var result = await _service.GetEventDetailsByIdAsync(id);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.StatusCode.Should().Be(404);
            result.Error.Should().Be("Event not found");
            result.Result.Should().BeNull();
        }
    }
}
