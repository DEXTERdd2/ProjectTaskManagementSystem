using GamePlanBackend.Application.Common.ResponseType;
using MediatR;
using ProjectManagementSystem.Application.MediaR.Dtos.AuthDtos;

namespace ProjectManagementSystem.Application.MediaR.Commands
{
    public class LoginUserCommand : IRequest<ResponseModel>
    {
        public LoginUserRequest LoginUserRequest { get; set; }

        public LoginUserCommand(LoginUserRequest request)
        {
            LoginUserRequest = request;
        }
    }
}
