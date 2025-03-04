using Events_WEB_APP.Core.Entities;
using Events_WEB_APP.Persistence.Data;
using Events_WEB_APP.Persistence.Repositories.CategoryRepository;
using Events_WEB_APP.Persistence.Repositories.EventRepository;
using Events_WEB_APP.Persistence.Repositories.ParticipantRepository;
using Events_WEB_APP.Persistence.Repositories.RoleRepository;
using Events_WEB_APP.Persistence.Repositories.UserRepository;
using Events_WEB_APP.Persistence.UnitsOfWork;
using Microsoft.EntityFrameworkCore;

public class EventUnitOfWorkTests : IDisposable
{
    private readonly EventsAppDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public EventUnitOfWorkTests()
    {
        var options = new DbContextOptionsBuilder<EventsAppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new EventsAppDbContext(options);
        _unitOfWork = new UnitOfWork(_context, new CategoryRepository(_context),
            new EventRepository(_context), new ParticipantRepository(_context),
            new RoleRepository(_context), new UserRepository(_context));
    }

    public void Dispose()
    {
        _unitOfWork.Dispose();
        _context.Dispose();
    }

    private async Task<Category> CreateTestCategoryAsync(string name)
    {
        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = name
        };

        await _unitOfWork.Categories.AddAsync(category);
        await _unitOfWork.CommitAsync();
        return category;
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnEvent_WhenExists()
    {
        // Arrange
        var testCategory = await CreateTestCategoryAsync("Test Category");

        var testEvent = new Event
        {
            Id = Guid.NewGuid(),
            Name = "Test Event",
            Date = DateTime.UtcNow.AddDays(1),
            Description = "Test Description",
            Location = "Test Location",
            CategoryId = testCategory.Id 
        };

        await _context.Events.AddAsync(testEvent);
        await _context.SaveChangesAsync();

        // Act
        var result = await _unitOfWork.Events.GetByIdAsync(testEvent.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(testEvent.Name, result.Name);
    }

    [Fact]
    public async Task UpdateAndCommit_ShouldModifyExistingEvent()
    {
        // Arrange
        var testCategory = await CreateTestCategoryAsync("Original Category");

        var existingEvent = new Event
        {
            Id = Guid.NewGuid(),
            Name = "Original Name",
            Date = DateTime.UtcNow.AddDays(3),
            Description = "Test Description",
            Location = "Original Location",
            CategoryId = testCategory.Id
        };

        await _context.Events.AddAsync(existingEvent);
        await _context.SaveChangesAsync();

        // Act
        existingEvent.Name = "Updated Name";
        await _unitOfWork.Events.UpdateAsync(existingEvent);
        await _unitOfWork.CommitAsync();

        // Assert
        var updatedEvent = await _unitOfWork.Events.GetByIdAsync(existingEvent.Id);
        Assert.Equal("Updated Name", updatedEvent.Name);
    }

 

   
}