using GamePlanBackend.Application.Common.ResponseType;

using ProjectManagementSystem.Application.DTOs.UserDtos;

namespace ProjectManagementSystem.Domain.Interference
{
    public interface IAuthService
    {
        Task<ResponseModel> RegisterAsync(RegisterRequest request);
        Task<ResponseModel> LoginAsync(LoginRequest request);
    }
}
