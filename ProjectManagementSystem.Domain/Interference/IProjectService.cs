
using GamePlanBackend.Application.Common.ResponseType;
using ProjectManagementSystem.Application.DTOs.ProjectDtos;

namespace ProjectManagementSystem.Domain.Interference
{
    public interface IProjectService
    {
        Task<ResponseModel> GetAllProjectsAsync(string? status = null, int page = 1, int pageSize = 10);
        Task<ResponseModel> GetProjectByIdAsync(int id);
        Task<ResponseModel> CreateProjectAsync(CreateProjectRequest request);
        Task<ResponseModel> UpdateProjectAsync(int id, UpdateProjectRequest request);
        Task<ResponseModel> DeleteProjectAsync(int id);
    }

}
