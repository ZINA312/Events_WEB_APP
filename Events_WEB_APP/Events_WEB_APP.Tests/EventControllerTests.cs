using AutoMapper;
using Events_WEB_APP.API.Controllers;
using Events_WEB_APP.Application.Services.EventService;
using Events_WEB_APP.Core.Entities;
using Events_WEB_APP.Persistence.Contracts;
using Events_WEB_APP.Persistence.Contracts.Event;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace Events_WEB_APP.API.Tests.Controllers
{
    public class EventControllerTests : IDisposable
    {
        private readonly Mock<IEventService> _mockEventService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IWebHostEnvironment> _mockWebHostEnv;
        private readonly EventController _controller;
        private readonly DefaultHttpContext _httpContext;
        public EventControllerTests()
        {
            _mockEventService = new Mock<IEventService>();
            _mockMapper = new Mock<IMapper>();
            _mockWebHostEnv = new Mock<IWebHostEnvironment>();
            _httpContext = new DefaultHttpContext();
            _mockWebHostEnv.Setup(e => e.WebRootPath).Returns("wwwroot");

            _controller = new EventController(
                _mockEventService.Object,
                _mockMapper.Object,
                _mockWebHostEnv.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = _httpContext
                }
            };
        }

        [Fact]
        public async Task Search_ReturnsEvents_WhenNameMatches()
        {
            // Arrange
            var testEvent = CreateTestEvent();
            testEvent.Name = "Test Conference";
            var paginated = new PaginatedResponse<Event>(
                new List<Event> { testEvent },
                1,
                10,
                1
            );

            _mockEventService.Setup(s =>
                s.SearchEventsByNameAsync("Conference", 1, 10))
                .ReturnsAsync(paginated);

            _mockMapper.Setup(m => m.Map<List<EventResponse>>(It.IsAny<List<Event>>()))
                .Returns(new List<EventResponse> { new EventResponse { Name = testEvent.Name } });

            // Act
            var result = await _controller.GetByName("Conference", 1, 10);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<PaginatedResponse<EventResponse>>(okResult.Value);
            Assert.Single(response.Items);
            Assert.Equal("Test Conference", response.Items.First().Name);
        }

        [Fact]
        public async Task Search_ReturnsBadRequest_WhenNameIsEmpty()
        {
            // Act
            var result = await _controller.GetByName("", 1, 10);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task Search_ReturnsEmpty_WhenNoMatches()
        {
            // Arrange
            var paginated = new PaginatedResponse<Event>(
                new List<Event>(),
                1,
                10,
                0
            );

            _mockEventService.Setup(s =>
                s.SearchEventsByNameAsync("Unknown", 1, 10))
                .ReturnsAsync(paginated);

            // Act
            var result = await _controller.GetByName("Unknown", 1, 10);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<PaginatedResponse<EventResponse>>(okResult.Value);
            Assert.Null(response.Items);
            Assert.Equal(0, response.TotalCount);
        }

        [Fact]
        public async Task Search_ReturnsCorrectPaginationMetadata()
        {
            // Arrange
            var events = new List<Event>
            {
                CreateTestEvent(),
                CreateTestEvent()
            };
            var paginated = new PaginatedResponse<Event>(events, 2, 1, 2);

            _mockEventService.Setup(s =>
                s.SearchEventsByNameAsync("Event", 2, 1))
                .ReturnsAsync(paginated);

            // Act
            var result = await _controller.GetByName("Event", 2, 1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<PaginatedResponse<EventResponse>>(okResult.Value);
            Assert.Equal(2, response.PageNumber);
            Assert.Equal(1, response.PageSize);
            Assert.Equal(2, response.TotalCount);
        }

        public void Dispose() { }

        private Event CreateTestEvent()
        {
            return new Event
            {
                Id = Guid.NewGuid(),
                Name = "Test Event",
                Description = "Test Description",
                Date = DateTime.UtcNow.AddDays(7),
                Location = "Test Location",
                CategoryId = Guid.NewGuid(),
                MaxNumOfParticipants = 100,
                ImagePath = "test.jpg"
            };
        }
        private void SetAdminUser()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Role, "Admin")
            }));
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        private void SetUnauthorizedUser()
        {
            _httpContext.User = new ClaimsPrincipal(); // Пустая аутентификация
        }

        [Fact]
        public async Task GetById_ReturnsEvent_WhenExists()
        {
            // Arrange
            var testEvent = CreateTestEvent();
            _mockEventService.Setup(s => s.GetEventByIdAsync(testEvent.Id))
                .ReturnsAsync(testEvent);

            _mockMapper.Setup(m => m.Map<EventResponse>(testEvent))
                .Returns(new EventResponse
                {
                    Id = testEvent.Id,
                    Name = testEvent.Name,
                    Date = testEvent.Date,
                    Location = testEvent.Location,
                    CategoryId = testEvent.CategoryId
                });

            // Act
            var result = await _controller.GetById(testEvent.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<EventResponse>(okResult.Value);
            Assert.Equal(testEvent.Id, response.Id);
        }

        [Fact]
        public async Task Create_ValidRequest_ReturnsCreatedEvent()
        {
            // Arrange
            var request = new EventCreateRequest
            (
                Name: "New Event",
                Description: "Description",
                Date: DateTime.UtcNow.AddDays(3),
                Location: "New Location",
                CategoryId: Guid.NewGuid(),
                MaxNumOfParticipants: 50
            );

            var newEvent = new Event
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Date = request.Date,
                Location = request.Location,
                CategoryId = request.CategoryId,
                MaxNumOfParticipants = request.MaxNumOfParticipants,
                ImagePath = null
            };

            _mockMapper.Setup(m => m.Map<Event>(request))
                .Returns(newEvent);

            // Act
            var result = await _controller.Create(request);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result.Result);
        }

        [Fact]
        public async Task Update_ValidRequest_UpdatesEvent()
        {
            // Arrange
            var existingEvent = CreateTestEvent();
            var request = new EventUpdateRequest
            (
                Id: existingEvent.Id,
                Name: "Updated Name",
                Description: "Updated Desc",
                Date: existingEvent.Date,
                Location: "Updated Location",
                CategoryId: existingEvent.CategoryId,
                MaxNumOfParticipants: 200
            );

            _mockEventService.Setup(s => s.GetEventByIdAsync(existingEvent.Id))
                .ReturnsAsync(existingEvent);

            _mockMapper.Setup(m => m.Map(request, existingEvent));

            // Act
            var result = await _controller.Update(existingEvent.Id, request);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task GetAll_ReturnsPaginatedEvents()
        {
            // Arrange
            var events = new List<Event> { CreateTestEvent() };
            var paginated = new PaginatedResponse<Event>(events, 1, 10, 1);

            _mockEventService.Setup(s => s.GetEventsPaginatedAsync(null, null, null, 1, 10))
                .ReturnsAsync(paginated);

            _mockMapper.Setup(m => m.Map<List<EventResponse>>(It.IsAny<List<Event>>()))
                .Returns(new List<EventResponse> { new EventResponse() });

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<PaginatedResponse<EventResponse>>(okResult.Value);
            Assert.Single(response.Items);
        }

        [Fact]
        public async Task Filter_WithCategory_ReturnsFilteredResults()
        {
            // Arrange
            var category = "Tech";
            var events = new List<Event> { CreateTestEvent() };
            var paginated = new PaginatedResponse<Event>(events, 1, 10, 1);

            _mockEventService.Setup(s => s.GetEventsPaginatedAsync(category, null, null, 1, 10))
                .ReturnsAsync(paginated);

            // Act
            var result = await _controller.Filter(category, null, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task UploadImage_InvalidFile_ReturnsBadRequest()
        {
            // Arrange
            SetAdminUser();
            var testEvent = CreateTestEvent();
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.Length).Returns(0);

            _mockEventService.Setup(s => s.GetEventByIdAsync(testEvent.Id))
                .ReturnsAsync(testEvent);

            // Act
            var result = await _controller.UploadImage(
                testEvent.Id,
                new ImageUploadRequest(mockFile.Object));

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task Create_InvalidData_ReturnsBadRequest()
        {
            // Arrange
            SetAdminUser();
            var invalidRequest = new EventCreateRequest(
            Name: "",
            Description: "Test",
            Date: DateTime.UtcNow.AddDays(-1),
            Location: "Test",
            CategoryId: Guid.NewGuid(),
            MaxNumOfParticipants: 0
            );
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.Create(invalidRequest);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task Update_NonExistingEvent_ReturnsNotFound()
        {
            // Arrange
            SetAdminUser();
            var request = new EventUpdateRequest(
                Id: Guid.NewGuid(),
                Name: "Updated",
                Description: "Desc",
                Date: DateTime.UtcNow,
                Location: "Loc",
                CategoryId: Guid.NewGuid(),
                MaxNumOfParticipants: 10
            );

            _mockEventService.Setup(s => s.GetEventByIdAsync(request.Id))
                .ReturnsAsync((Event)null);

            // Act
            var result = await _controller.Update(request.Id, request);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetById_NonExistingEvent_ReturnsNotFound()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();
            _mockEventService.Setup(s => s.GetEventByIdAsync(nonExistingId))
                .ReturnsAsync((Event)null);

            // Act
            var result = await _controller.GetById(nonExistingId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Update_IdMismatch_ReturnsBadRequest()
        {
            // Arrange
            SetAdminUser();
            var request = new EventUpdateRequest
            (
                Id: Guid.NewGuid(),
                Name: "New Event",
                Description: "Description",
                Date: DateTime.UtcNow.AddDays(3),
                Location: "New Location",
                CategoryId: Guid.NewGuid(),
                MaxNumOfParticipants: 50
            );

            // Act
            var result = await _controller.Update(Guid.NewGuid(), request);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Delete_ExistingEventAsAdmin_ReturnsNoContent()
        {
            // Arrange
            SetAdminUser();
            var eventId = Guid.NewGuid();

            _mockEventService.Setup(s => s.DeleteEventAsync(eventId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(eventId);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockEventService.Verify(s => s.DeleteEventAsync(eventId), Times.Once);
        }

        [Fact]
        public async Task Delete_NonExistingEvent_ReturnsNotFound()
        {
            // Arrange
            SetAdminUser();
            var nonExistingId = Guid.NewGuid();

            _mockEventService.Setup(s => s.DeleteEventAsync(nonExistingId))
                .ThrowsAsync(new KeyNotFoundException("Event not found"));

            // Act
            var result = await _controller.Delete(nonExistingId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Event not found", notFoundResult.Value);
        }

        [Fact]
        public async Task Delete_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            SetAdminUser();
            var invalidId = Guid.Empty;

            _mockEventService.Setup(s => s.DeleteEventAsync(invalidId))
                .ThrowsAsync(new ArgumentException("Invalid ID"));

            // Act
            var result = await _controller.Delete(invalidId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid ID", badRequestResult.Value);
        }
    }
}