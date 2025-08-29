using employee_confirmation_api_final.DTOs;
using employee_confirmation_api_final.Services;
using Microsoft.AspNetCore.Mvc;

namespace employee_confirmation_api_final.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeConfirmationController : ControllerBase
    {
        private readonly EmployeeConfirmationService _service;

        public EmployeeConfirmationController(EmployeeConfirmationService service)
        {
            _service = service;
        }

        [HttpGet("GetEmployeesForConfirmation")]
        public async Task<ActionResult<List<EmployeeForConfirmationDto>>> GetEmployeesForConfirmation()
        {
            var employees = await _service.GetEmployeesForConfirmationAsync();
            return Ok(employees);
        }

        // NEW POST ENDPOINT
        [HttpPost("InsertEmployeeConfirmation")]
        public async Task<ActionResult<EmployeeConfirmationInsertResponseDto>> InsertEmployeeConfirmation([FromBody] EmployeeConfirmationInsertDto dto)
        {
            var result = await _service.InsertEmployeeConfirmationAsync(dto);
            return Ok(result);
        }
        [HttpGet("GetDetailsByMasterId/{ecid}")]
        public async Task<ActionResult<ConfirmationMasterDetailsResponseDto>> GetConfirmationDetailsByMasterId(int ecid)
        {
            var details = await _service.GetConfirmationDetailsByMasterIdAsync(ecid);
            return Ok(details);
        }
        [HttpGet("GetProjects")]
        public async Task<ActionResult<List<ProjectDto>>> GetProjects()
        {
            var projects = await _service.GetProjectsAsync();
            return Ok(projects);
        }
        [HttpGet("GetExtensionDetails")]
        public async Task<ActionResult<List<ExtensionDetailDto>>> GetExtensionDetails([FromQuery] int mempId, [FromQuery] int instanceId)
        {
            var history = await _service.GetExtensionDetailsAsync(mempId, instanceId);
            return Ok(history);
        }
        [HttpPost("SaveRmFeedback")]
        public async Task<ActionResult<bool>> SaveRmFeedback([FromBody] SaveRmFeedbackDto dto)
        {
            var result = await _service.SaveRmFeedbackAsync(dto);
            return Ok(result);
        }
        [HttpPost("SubmitRmDecision")]
        public async Task<ActionResult<bool>> SubmitRmDecision([FromBody] SubmitRmDecisionDto dto)
        {
            // The service layer will handle validation
            var result = await _service.SubmitRmDecisionAsync(dto);
            return Ok(result);
        }
        [HttpPost("SaveHeadFeedback")]
        public async Task<ActionResult<bool>> SaveHeadFeedback([FromBody] SaveHeadFeedbackDto dto)
        {
            var result = await _service.SaveHeadFeedbackAsync(dto);
            return Ok(result);
        }

        [HttpPost("SubmitGhDecision")]
        public async Task<ActionResult<bool>> SubmitGhDecision([FromBody] SubmitGhDecisionDto dto)
        {
            var result = await _service.SubmitGhDecisionAsync(dto);
            return Ok(result);
        }
    }
}
