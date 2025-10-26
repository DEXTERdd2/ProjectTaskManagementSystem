using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Application.DTOs.ProjectDtos;
using ProjectManagementSystem.Infrastructure.Data;
using ProjectManagementSystem.Infrastructure.Services.ProjectService;

public class ProjectServiceTests : IAsyncLifetime
{
    private AppDbContext _context;
    private ProjectService _service;

    public async Task InitializeAsync()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new AppDbContext(options);

        // Seed initial data
        var project1 = new Tb_Project
        {
            Name = "Project 1",
            Description = "Desc 1",
            StartDate = DateTime.UtcNow,
            Status = ProjectStatus.Planned
        };

        var project2 = new Tb_Project
        {
            Name = "Project 2",
            Description = "Desc 2",
            StartDate = DateTime.UtcNow,
            Status = ProjectStatus.Active
        };

        _context.Tb_Projects.AddRange(project1, project2);
        await _context.SaveChangesAsync();

        _service = new ProjectService(_context);
    }

    public async Task DisposeAsync()
    {
        await _context.Database.EnsureDeletedAsync();
        await _context.DisposeAsync();
    }

    [Fact]
    public async Task GetAllProjectsAsync_ShouldReturnAllProjects()
    {
        var result = await _service.GetAllProjectsAsync();
        Assert.Equal(200, result.statusCode);
        var projects = result.DataModel as System.Collections.Generic.List<ProjectDto>;
        Assert.NotNull(projects);
        Assert.Equal(2, projects.Count);
    }

    [Fact]
    public async Task GetAllProjectsAsync_WithStatusFilter_ShouldReturnFilteredProjects()
    {
        var result = await _service.GetAllProjectsAsync("Active");
        var projects = result.DataModel as System.Collections.Generic.List<ProjectDto>;
        Assert.Single(projects);
        Assert.Equal("Project 2", projects.First().Name);
    }

    [Fact]
    public async Task GetProjectByIdAsync_ShouldReturnProject_WhenExists()
    {
        var projectId = _context.Tb_Projects.First().Id;
        var result = await _service.GetProjectByIdAsync(projectId);
        Assert.Equal(200, result.statusCode);
        var project = result.DataModel as ProjectDto;
        Assert.NotNull(project);
        Assert.Equal("Project 1", project.Name);
    }

    [Fact]
    public async Task GetProjectByIdAsync_ShouldReturnNotFound_WhenDoesNotExist()
    {
        var result = await _service.GetProjectByIdAsync(999);
        Assert.Equal(400, result.statusCode);
        Assert.Equal("Project not found.", result.ResponseMessage);
    }

    [Fact]
    public async Task CreateProjectAsync_ShouldCreateProject()
    {
        var request = new CreateProjectRequest
        {
            Name = "New Project",
            Description = "New Desc",
            StartDate = DateTime.UtcNow
        };

        var result = await _service.CreateProjectAsync(request);
        Assert.Equal(200, result.statusCode);
        var project = result.DataModel as ProjectDto;
        Assert.NotNull(project);
        Assert.Equal("New Project", project.Name);
        Assert.Equal(3, _context.Tb_Projects.Count());
    }

    [Fact]
    public async Task UpdateProjectAsync_ShouldUpdateProject()
    {
        var projectId = _context.Tb_Projects.First().Id;
        var request = new UpdateProjectRequest
        {
            Name = "Updated Name",
            Status = "Completed"
        };

        var result = await _service.UpdateProjectAsync(projectId, request);
        Assert.Equal(200, result.statusCode);
        var project = _context.Tb_Projects.Find(projectId);
        Assert.Equal("Updated Name", project.Name);
        Assert.Equal(ProjectStatus.Completed, project.Status);
    }

    [Fact]
    public async Task DeleteProjectAsync_ShouldDeleteProject()
    {
        var projectId = _context.Tb_Projects.First().Id;
        var result = await _service.DeleteProjectAsync(projectId);
        Assert.Equal(200, result.statusCode);
        Assert.Null(_context.Tb_Projects.Find(projectId));
    }
}
