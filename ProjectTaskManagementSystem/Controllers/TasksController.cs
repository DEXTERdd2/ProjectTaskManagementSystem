using GamePlanBackend.Application.Common.ResponseType;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Application.DTOs.TaskItemDtos;
using ProjectManagementSystem.Domain.Interference;

namespace ProjectManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,User")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? projectId, [FromQuery] string? status, [FromQuery] string? assignedTo)
        {
            try { 
            var result = await _taskService.GetAllTasksAsync(projectId, status, assignedTo);
            return StatusCode(result.statusCode, result);
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseModel
                {
                    statusCode = 500,
                    ResponseMessage = "Error retrieving tasks: " + ex.Message,
                    IsError = true,
                    Success = false
                };
                return StatusCode(500, errorResponse);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try {
            var result = await _taskService.GetTaskByIdAsync(id);
            return StatusCode(result.statusCode, result);
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseModel
                {
                    statusCode = 500,
                    ResponseMessage = "Error retrieving task: " + ex.Message,
                    IsError = true,
                    Success = false
                };
                return StatusCode(500, errorResponse);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskRequest request)
        {
            try { 
            var result = await _taskService.CreateTaskAsync(request);
            return StatusCode(result.statusCode, result);
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseModel
                {
                    statusCode = 500,
                    ResponseMessage = "Error creating task: " + ex.Message,
                    IsError = true,
                    Success = false
                };
                return StatusCode(500, errorResponse);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskRequest request)
        {
            try
            {
                var result = await _taskService.UpdateTaskAsync(id, request);
                return StatusCode(result.statusCode, result);
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseModel
                {
                    statusCode = 500,
                    ResponseMessage = "Error updating task: " + ex.Message,
                    IsError = true,
                    Success = false
                };
                return StatusCode(500, errorResponse);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _taskService.DeleteTaskAsync(id);
                return StatusCode(result.statusCode, result);
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseModel
                {
                    statusCode = 500,
                    ResponseMessage = "Error deleting task: " + ex.Message,
                    IsError = true,
                    Success = false
                };
                return StatusCode(500, errorResponse);
            }
        }
    }
}
