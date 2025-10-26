using GamePlanBackend.Application.Common.MiddleWare;
using GamePlanBackend.Application.Common.ResponseType;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Application.DTOs.TaskItemDtos;
using ProjectManagementSystem.Domain.Interference;
using ProjectManagementSystem.Infrastructure.Data;


namespace ProjectManagementSystem.Infrastructure.Services.TaskService
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _context;

        public TaskService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel> GetAllTasksAsync(int? projectId = null, string? status = null, string? assignedTo = null)
        {
            try
            {
                var query = _context.Tb_TaskItems.AsQueryable();

                if (projectId.HasValue)
                    query = query.Where(t => t.ProjectId == projectId.Value);

                if (!string.IsNullOrWhiteSpace(assignedTo))
                    query = query.Where(t => t.AssignedTo == assignedTo);


                TaskStatus? taskStatus = null;
                if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<TaskStatus>(status, true, out var parsedStatus))
                {
                    taskStatus = parsedStatus;
                    query = query.Where(t => t.Status == taskStatus.Value);
                }

                var tasks = await query
                    .Select(t => new TaskItemDto
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Description = t.Description,
                        AssignedTo = t.AssignedTo,
                        DueDate = t.DueDate,
                        Status = t.Status.ToString(),
                        CreatedAt = t.CreatedAt,
                        ProjectId = t.ProjectId
                    })
                    .ToListAsync();

                return ResponseData.GetSuccessResponse(tasks, "Tasks fetched successfully.");
            }
            catch (Exception ex)
            {
                return ResponseData.ErrorResponse("Failed to fetch tasks", ex.Message);
            }
        }


        public async Task<ResponseModel> GetTaskByIdAsync(int id)
        {
            try
            {
                var task = await _context.Tb_TaskItems
                    .Where(t => t.Id == id)
                    .Select(t => new TaskItemDto
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Description = t.Description,
                        AssignedTo = t.AssignedTo,
                        DueDate = t.DueDate,
                        Status = t.Status.ToString(),
                        CreatedAt = t.CreatedAt,
                        ProjectId = t.ProjectId
                    })
                    .FirstOrDefaultAsync();

                if (task == null)
                    return ResponseData.NotSuccessResponse("Task not found.");

                return ResponseData.GetSuccessResponse(task, "Task fetched successfully.");
            }
            catch (Exception ex)
            {
                return ResponseData.ErrorResponse("Failed to fetch task", ex.Message);
            }
        }

        public async Task<ResponseModel> CreateTaskAsync(CreateTaskRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Title))
                    return ResponseData.NotSuccessResponse("Title is required.");

                var project = await _context.Tb_Projects.FindAsync(request.ProjectId);
                if (project == null)
                    return ResponseData.NotSuccessResponse("Project not found.");

                var task = new Tb_TaskItem
                {
                    Title = request.Title,
                    Description = request.Description,
                    AssignedTo = request.AssignedTo,
                    DueDate = request.DueDate,
                    Status = Enum.TryParse<TaskStatus>(request.Status, out var status) ? status : TaskStatus.Pending,
                    ProjectId = request.ProjectId,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Tb_TaskItems.Add(task);
                await _context.SaveChangesAsync();

                return ResponseData.GetSuccessResponse(task.Id, "Task created successfully.");
            }
            catch (Exception ex)
            {
                return ResponseData.ErrorResponse("Failed to create task", ex.Message);
            }
        }

        public async Task<ResponseModel> UpdateTaskAsync(int id, UpdateTaskRequest request)
        {
            try
            {
                var task = await _context.Tb_TaskItems.FindAsync(id);
                if (task == null)
                    return ResponseData.NotSuccessResponse("Task not found.");

                if (!string.IsNullOrWhiteSpace(request.Title))
                    task.Title = request.Title;
                if (!string.IsNullOrWhiteSpace(request.Description))
                    task.Description = request.Description;
                if (!string.IsNullOrWhiteSpace(request.AssignedTo))
                    task.AssignedTo = request.AssignedTo;
                if (request.DueDate.HasValue)
                    task.DueDate = request.DueDate;
                if (!string.IsNullOrWhiteSpace(request.Status) && Enum.TryParse<TaskStatus>(request.Status, out var status))
                    task.Status = status;

                await _context.SaveChangesAsync();

                return ResponseData.GetSuccessResponse(task.Id, "Task updated successfully.");
            }
            catch (Exception ex)
            {
                return ResponseData.ErrorResponse("Failed to update task", ex.Message);
            }
        }

        public async Task<ResponseModel> DeleteTaskAsync(int id)
        {
            try
            {
                var task = await _context.Tb_TaskItems.FindAsync(id);
                if (task == null)
                    return ResponseData.NotSuccessResponse("Task not found.");

                _context.Tb_TaskItems.Remove(task);
                await _context.SaveChangesAsync();

                return ResponseData.GetSuccessResponse(task.Id, "Task deleted successfully.");
            }
            catch (Exception ex)
            {
                return ResponseData.ErrorResponse("Failed to delete task", ex.Message);
            }
        }
    }

}
