using GamePlanBackend.Application.Common.ResponseType;
using ProjectManagementSystem.Application.DTOs.TaskItemDtos;

namespace ProjectManagementSystem.Domain.Interference
{
    public interface ITaskService
    {
        Task<ResponseModel> GetAllTasksAsync(int? projectId = null, string? status = null, string? assignedTo = null);
        Task<ResponseModel> GetTaskByIdAsync(int id);
        Task<ResponseModel> CreateTaskAsync(CreateTaskRequest request);
        Task<ResponseModel> UpdateTaskAsync(int id, UpdateTaskRequest request);
        Task<ResponseModel> DeleteTaskAsync(int id);
    }

}
