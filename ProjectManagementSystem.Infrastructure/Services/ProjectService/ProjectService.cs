
using GamePlanBackend.Application.Common.MiddleWare;
using GamePlanBackend.Application.Common.ResponseType;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Application.DTOs.ProjectDtos;
using ProjectManagementSystem.Domain.Interference;
using ProjectManagementSystem.Infrastructure.Data;

namespace ProjectManagementSystem.Infrastructure.Services.ProjectService
{
    public class ProjectService : IProjectService
    {
        private readonly AppDbContext _context;

        public ProjectService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel> GetAllProjectsAsync(string? status = null, int page = 1, int pageSize = 10)
        {
            try
            {
                var query = _context.Tb_Projects.Include(p => p.TaskItems).AsQueryable();

                if (!string.IsNullOrEmpty(status) && Enum.TryParse<ProjectStatus>(status, true, out var projectStatus))
                {
                    query = query.Where(p => p.Status == projectStatus);
                }

                var projects = await query
                    .OrderByDescending(p => p.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var data = projects.Select(p => new ProjectDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    Status = p.Status.ToString(),
                    CreatedAt = p.CreatedAt,
                    Tasks = p.TaskItems?.Select(t => new TaskItemDto
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Description = t.Description,
                        AssignedTo = t.AssignedTo,
                        DueDate = t.DueDate,
                        Status = t.Status.ToString(),
                        CreatedAt = t.CreatedAt
                    }).ToList()
                }).ToList();

                return new ResponseModel
                {
                    statusCode = 200,
                    ResponseMessage = "Projects retrieved successfully.",
                    DataModel = data
                };
            }
            catch (Exception ex)
            {
                return ResponseData.ErrorResponse("Failed to get projects", ex.Message);
            }
        }

        public async Task<ResponseModel> GetProjectByIdAsync(int id)
        {
            try
            {
                var project = await _context.Tb_Projects.Include(p => p.TaskItems).FirstOrDefaultAsync(p => p.Id == id);
                if (project == null)
                    return ResponseData.NotSuccessResponse("Project not found.");

                var data = new ProjectDto
                {
                    Id = project.Id,
                    Name = project.Name,
                    Description = project.Description,
                    StartDate = project.StartDate,
                    EndDate = project.EndDate,
                    Status = project.Status.ToString(),
                    CreatedAt = project.CreatedAt,
                    Tasks = project.TaskItems?.Select(t => new TaskItemDto
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Description = t.Description,
                        AssignedTo = t.AssignedTo,
                        DueDate = t.DueDate,
                        Status = t.Status.ToString(),
                        CreatedAt = t.CreatedAt
                    }).ToList()
                };

                return new ResponseModel
                {
                    statusCode = 200,
                    ResponseMessage = "Project retrieved successfully.",
                    DataModel = data
                };
            }
            catch (Exception ex)
            {
                return ResponseData.ErrorResponse("Failed to get project", ex.Message);
            }
        }

        public async Task<ResponseModel> CreateProjectAsync(CreateProjectRequest request)
        {
            try
            {
                var project = new Tb_Project
                {
                    Name = request.Name,
                    Description = request.Description,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    Status = request.Status != null && Enum.TryParse<ProjectStatus>(request.Status, true, out var s)
                        ? s
                        : ProjectStatus.Planned
                };

                _context.Tb_Projects.Add(project);
                await _context.SaveChangesAsync();

                var data = new ProjectDto
                {
                    Id = project.Id,
                    Name = project.Name,
                    Description = project.Description,
                    StartDate = project.StartDate,
                    EndDate = project.EndDate,
                    Status = project.Status.ToString(),
                    CreatedAt = project.CreatedAt,
                    Tasks = new List<TaskItemDto>()
                };

                return new ResponseModel
                {
                    statusCode = 200,
                    ResponseMessage = "Project created successfully.",
                    DataModel = data
                };
            }
            catch (Exception ex)
            {
                return ResponseData.ErrorResponse("Project creation failed", ex.Message);
            }
        }

        public async Task<ResponseModel> UpdateProjectAsync(int id, UpdateProjectRequest request)
        {
            try
            {
                var project = await _context.Tb_Projects.FindAsync(id);
                if (project == null)
                    return ResponseData.NotSuccessResponse("Project not found.");

                if (!string.IsNullOrEmpty(request.Name)) project.Name = request.Name;
                if (!string.IsNullOrEmpty(request.Description)) project.Description = request.Description;
                if (request.StartDate.HasValue) project.StartDate = request.StartDate.Value;
                if (request.EndDate.HasValue) project.EndDate = request.EndDate.Value;
                if (!string.IsNullOrEmpty(request.Status) && Enum.TryParse<ProjectStatus>(request.Status, true, out var status))
                    project.Status = status;

                await _context.SaveChangesAsync();

                var data = new ProjectDto
                {
                    Id = project.Id,
                    Name = project.Name,
                    Description = project.Description,
                    StartDate = project.StartDate,
                    EndDate = project.EndDate,
                    Status = project.Status.ToString(),
                    CreatedAt = project.CreatedAt,
                    Tasks = project.TaskItems?.Select(t => new TaskItemDto
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Description = t.Description,
                        AssignedTo = t.AssignedTo,
                        DueDate = t.DueDate,
                        Status = t.Status.ToString(),
                        CreatedAt = t.CreatedAt
                    }).ToList()
                };

                return new ResponseModel
                {
                    statusCode = 200,
                    ResponseMessage = "Project updated successfully.",
                    DataModel = data
                };
            }
            catch (Exception ex)
            {
                return ResponseData.ErrorResponse("Project update failed", ex.Message);
            }
        }

        public async Task<ResponseModel> DeleteProjectAsync(int id)
        {
            try
            {
                var project = await _context.Tb_Projects.Include(p => p.TaskItems).FirstOrDefaultAsync(p => p.Id == id);
                if (project == null)
                    return ResponseData.NotSuccessResponse("Project not found.");

                _context.Tb_Projects.Remove(project);
                await _context.SaveChangesAsync();

                return new ResponseModel
                {
                    statusCode = 200,
                    ResponseMessage = "Project deleted successfully.",
                    DataModel = null
                };
            }
            catch (Exception ex)
            {
                return ResponseData.ErrorResponse("Project deletion failed", ex.Message);
            }
        }
    }

}
