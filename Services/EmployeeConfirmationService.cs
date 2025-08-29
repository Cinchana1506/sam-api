using employee_confirmation_api_final.DTOs;
using employee_confirmation_api_final.Interfaces;

namespace employee_confirmation_api_final.Services
{
    public class EmployeeConfirmationService
    {
        private readonly IEmployeeConfirmationRepository _repo;

        public EmployeeConfirmationService(IEmployeeConfirmationRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<EmployeeForConfirmationDto>> GetEmployeesForConfirmationAsync()
        {
            return await _repo.GetEmployeesForConfirmationAsync();
        }

        // NEW METHOD
        public async Task<EmployeeConfirmationInsertResponseDto> InsertEmployeeConfirmationAsync(EmployeeConfirmationInsertDto dto)
        {
            return await _repo.InsertEmployeeConfirmationAsync(dto);
        }
        public async Task<ConfirmationMasterDetailsResponseDto> GetConfirmationDetailsByMasterIdAsync(int ecid)
        {
            return await _repo.GetConfirmationDetailsByMasterIdAsync(ecid);
        }
        public async Task<List<ProjectDto>> GetProjectsAsync()
        {
            return await _repo.GetProjectsAsync();
        }
        public async Task<List<ExtensionDetailDto>> GetExtensionDetailsAsync(int mempId, int instanceId)
        {
            return await _repo.GetExtensionDetailsAsync(mempId, instanceId);
        }
        public async Task<bool> SaveRmFeedbackAsync(SaveRmFeedbackDto dto)
        {
            return await _repo.SaveRmFeedbackAsync(dto);
        }
        public async Task<bool> SubmitRmDecisionAsync(SubmitRmDecisionDto dto)
        {
            // TODO: Add your complex validation rules here based on the requirements.
            // Example: 
            // - If "Extend" is selected, check for a valid reason and extension period.
            // - If "Confirm" is selected, ensure a justification is provided.
            // - Validate that all evaluation ratings are filled out.

            // For now, we just pass the data to the repository.
            // You must implement the validation logic as per your business rules.
            return await _repo.SubmitRmDecisionAsync(dto);
        }
        public async Task<bool> SaveHeadFeedbackAsync(SaveHeadFeedbackDto dto)
        {
            return await _repo.SaveHeadFeedbackAsync(dto);
        }

        public async Task<bool> SubmitGhDecisionAsync(SubmitGhDecisionDto dto)
        {
            // Add any GH-specific validation logic here
            return await _repo.SubmitGhDecisionAsync(dto);
        }
    }
}
