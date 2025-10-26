using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Application.DTOs.TaskItemDtos;
using ProjectManagementSystem.Infrastructure.Data;
using ProjectManagementSystem.Infrastructure.Services.TaskService;

public class TaskServiceTests : IAsyncLifetime
{
    private AppDbContext _context;
    private TaskService _service;

    public async Task InitializeAsync()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);

        // Seed Projects
        var project = new Tb_Project
        {
            Name = "Test Project",
            StartDate = DateTime.UtcNow,
            Status = ProjectStatus.Active
        };
        _context.Tb_Projects.Add(project);
        await _context.SaveChangesAsync();

        // Seed Tasks
        _context.Tb_TaskItems.AddRange(
            new Tb_TaskItem
            {
                Title = "Task 1",
                ProjectId = project.Id,
                Status = TaskStatus.Pending,
                AssignedTo = "user1",
                CreatedAt = DateTime.UtcNow
            },
            new Tb_TaskItem
            {
                Title = "Task 2",
                ProjectId = project.Id,
                Status = TaskStatus.InProgress,
                AssignedTo = "user2",
                CreatedAt = DateTime.UtcNow
            }
        );
        await _context.SaveChangesAsync();

        _service = new TaskService(_context); 
    }

    public async Task DisposeAsync()
    {
        await _context.Database.EnsureDeletedAsync();
        await _context.DisposeAsync();
    }

    [Fact]
    public async Task GetAllTasksAsync_ShouldReturnAllTasks()
    {
        var result = await _service.GetAllTasksAsync();
        Assert.Equal(200, result.statusCode);
        var tasks = result.DataModel as System.Collections.Generic.List<TaskItemDto>;
        Assert.NotNull(tasks);
        Assert.Equal(2, tasks.Count);
    }

    [Fact]
    public async Task GetAllTasksAsync_FilterByProjectId_ShouldReturnFilteredTasks()
    {
        var projectId = _context.Tb_Projects.First().Id;
        var result = await _service.GetAllTasksAsync(projectId: projectId);
        var tasks = result.DataModel as System.Collections.Generic.List<TaskItemDto>;
        Assert.Equal(2, tasks.Count);
    }

    [Fact]
    public async Task GetAllTasksAsync_FilterByStatus_ShouldReturnFilteredTasks()
    {
        var result = await _service.GetAllTasksAsync(status: "Pending");
        var tasks = result.DataModel as System.Collections.Generic.List<TaskItemDto>;
        Assert.Single(tasks);
        Assert.Equal("Task 1", tasks.First().Title);
    }

    [Fact]
    public async Task GetAllTasksAsync_FilterByAssignedTo_ShouldReturnFilteredTasks()
    {
        var result = await _service.GetAllTasksAsync(assignedTo: "user2");
        var tasks = result.DataModel as System.Collections.Generic.List<TaskItemDto>;
        Assert.Single(tasks);
        Assert.Equal("Task 2", tasks.First().Title);
    }

    [Fact]
    public async Task GetTaskByIdAsync_ShouldReturnTask_WhenExists()
    {
        var taskId = _context.Tb_TaskItems.First().Id;
        var result = await _service.GetTaskByIdAsync(taskId);
        Assert.Equal(200, result.statusCode);
        var task = result.DataModel as TaskItemDto;
        Assert.NotNull(task);
        Assert.Equal("Task 1", task.Title);
    }

    [Fact]
    public async Task GetTaskByIdAsync_ShouldReturnNotFound_WhenDoesNotExist()
    {
        var result = await _service.GetTaskByIdAsync(999);
        Assert.Equal(400, result.statusCode);
        Assert.Equal("Task not found.", result.ResponseMessage);
    }

    [Fact]
    public async Task CreateTaskAsync_ShouldCreateTask()
    {
        var projectId = _context.Tb_Projects.First().Id;
        var request = new CreateTaskRequest
        {
            Title = "New Task",
            ProjectId = projectId,
            Status = "Pending",
            AssignedTo = "user3"
        };

        var result = await _service.CreateTaskAsync(request);
        Assert.Equal(200, result.statusCode);
        var task = _context.Tb_TaskItems.FirstOrDefault(t => t.Title == "New Task");
        Assert.NotNull(task);
        Assert.Equal(TaskStatus.Pending, task.Status);
        Assert.Equal("user3", task.AssignedTo);
    }

    [Fact]
    public async Task UpdateTaskAsync_ShouldUpdateTask()
    {
        var taskId = _context.Tb_TaskItems.First().Id;
        var request = new UpdateTaskRequest
        {
            Title = "Updated Task",
            Status = "Done",
            AssignedTo = "user5"
        };

        var result = await _service.UpdateTaskAsync(taskId, request);
        Assert.Equal(200, result.statusCode);
        var task = _context.Tb_TaskItems.Find(taskId);
        Assert.Equal("Updated Task", task.Title);
        Assert.Equal(TaskStatus.Done, task.Status);
        Assert.Equal("user5", task.AssignedTo);
    }

    [Fact]
    public async Task DeleteTaskAsync_ShouldDeleteTask()
    {
        var taskId = _context.Tb_TaskItems.First().Id;
        var result = await _service.DeleteTaskAsync(taskId);
        Assert.Equal(200, result.statusCode);
        Assert.Null(_context.Tb_TaskItems.Find(taskId));
    }
}
