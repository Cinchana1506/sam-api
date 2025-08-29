using employee_confirmation_api_final.DTOs;

    namespace employee_confirmation_api_final.Interfaces
    {
    public interface IEmployeeConfirmationRepository
    {
        Task<List<EmployeeForConfirmationDto>> GetEmployeesForConfirmationAsync();
        Task<EmployeeConfirmationInsertResponseDto> InsertEmployeeConfirmationAsync(EmployeeConfirmationInsertDto dto);
        Task<ConfirmationMasterDetailsResponseDto> GetConfirmationDetailsByMasterIdAsync(int ecid);
        Task<List<ProjectDto>> GetProjectsAsync();

        // NEW METHOD for getting extension history
        Task<List<ExtensionDetailDto>> GetExtensionDetailsAsync(int mempId, int instanceId);
        Task<bool> SaveRmFeedbackAsync(SaveRmFeedbackDto dto);
        Task<bool> SubmitRmDecisionAsync(SubmitRmDecisionDto dto);
        Task<bool> SaveHeadFeedbackAsync(SaveHeadFeedbackDto dto);
        Task<bool> SubmitGhDecisionAsync(SubmitGhDecisionDto dto);
        }
    }


