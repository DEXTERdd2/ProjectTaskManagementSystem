using GamePlanBackend.Application.Common.ResponseType;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Application.DTOs.ProjectDtos;
using ProjectManagementSystem.Domain.Interference;

namespace ProjectManagementSystem.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,User")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? status, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await _projectService.GetAllProjectsAsync(status, page, pageSize);
                return StatusCode(result.statusCode, result);
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseModel
                {
                    statusCode = 500,
                    ResponseMessage = "Error retrieving projects: " + ex.Message,
                    IsError = true,
                    Success = false
                };
                return StatusCode(500, errorResponse);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _projectService.GetProjectByIdAsync(id);
                return StatusCode(result.statusCode, result);
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseModel
                {
                    statusCode = 500,
                    ResponseMessage = "Error retrieving project: " + ex.Message,
                    IsError = true,
                    Success = false
                };
                return StatusCode(500, errorResponse);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProjectRequest request)
        {
            try
            {
                var result = await _projectService.CreateProjectAsync(request);
                return StatusCode(result.statusCode, result);
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseModel
                {
                    statusCode = 500,
                    ResponseMessage = "Error creating project: " + ex.Message,
                    IsError = true,
                    Success = false
                };
                return StatusCode(500, errorResponse);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProjectRequest request)
        {
            try
            {
                var result = await _projectService.UpdateProjectAsync(id, request);
                return StatusCode(result.statusCode, result);
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseModel
                {
                    statusCode = 500,
                    ResponseMessage = "Error updating project: " + ex.Message,
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
                var result = await _projectService.DeleteProjectAsync(id);
                return StatusCode(result.statusCode, result);
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseModel
                {
                    statusCode = 500,
                    ResponseMessage = "Error deleting project: " + ex.Message,
                    IsError = true,
                    Success = false
                };
                return StatusCode(500, errorResponse);
            }
        }
    }

}
