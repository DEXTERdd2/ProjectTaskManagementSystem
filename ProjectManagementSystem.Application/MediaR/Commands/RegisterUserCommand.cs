using GamePlanBackend.Application.Common.ResponseType;
using MediatR;
using ProjectManagementSystem.Application.MediaR.Dtos.AuthDtos;

// Register Command
public class RegisterUserCommand : IRequest<ResponseModel>
{
    public RegisterUserRequest RegisterUserRequest { get; set; }

    public RegisterUserCommand(RegisterUserRequest request)
    {
        RegisterUserRequest = request;
    }
}