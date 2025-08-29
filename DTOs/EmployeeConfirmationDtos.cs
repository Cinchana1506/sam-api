namespace employee_confirmation_api_final.DTOs
{
    public class EmployeeForConfirmationDto
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public DateTime JoiningDate { get; set; }
    }

    // NEW DTO for Insert
    public class EmployeeConfirmationInsertDto
    {
        public int EmpId { get; set; }
        public int ProjectId { get; set; }
        public DateTime ConfirmDate { get; set; }
    }

    // Response DTO (ECID returned)
    public class EmployeeConfirmationInsertResponseDto
    {
        public int ECID { get; set; }
    }
    // Add these classes to the employee_confirmation_api_final.DTOs namespace

public class ConfirmationDetailsDto
{
    public int MEmpId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string EmployeeId { get; set; } = string.Empty;
    public string EID { get; set; } = string.Empty;
    public int IsForConfirmation { get; set; } // Use int if it's a status code, or bool if it's 1/0 for true/false
    public int? Extensions { get; set; }
    public string? JustificationGMReason { get; set; }
    public bool IsNWInitiated { get; set; }
    public int? MProjectId { get; set; }
    public bool IsConfirmed { get; set; }
    public DateTime? WithEffective { get; set; }
    public string ProjectName { get; set; } = "Not In List/Not Applicable";
    public int? WFStatus { get; set; }
    public DateTime? ExtensionFrom { get; set; }
    public bool? ISCampusRecruit { get; set; }
    public bool IsWhObjected { get; set; }
    public bool IsExtendedLastTime { get; set; }
    public int? NoOfDays { get; set; }
    // Add other fields from the SP result as needed
}

public class EvaluationParameterDto
{
    public int ParamId { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Parameter { get; set; } = string.Empty;
    public string ParameterDescription { get; set; } = string.Empty;
    public string RMRating { get; set; } = "0"; // SP returns VARCHAR
    public string RMRemarks { get; set; } = "";
    public string HeadRating { get; set; } = "0"; // SP returns VARCHAR
    public string HeadRemarks { get; set; } = "";
    public int ParameterOrder { get; set; }
}

    public class ConfirmationMasterDetailsResponseDto
    {
        public ConfirmationDetailsDto MasterDetails { get; set; } = new();
        public List<EvaluationParameterDto> EvaluationParameters { get; set; } = new();
        public decimal AverageRating { get; set; }
    }
    // Add this class to the employee_confirmation_api_final.DTOs namespace

    public class ProjectDto
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        // Add other relevant fields like ProjectCode, IsActive, etc., if needed for the frontend
    }
    // Add this class to the employee_confirmation_api_final.DTOs namespace

    public class ExtensionDetailDto
    {
        public DateTime ExtendedFrom { get; set; }
        public DateTime ExtendedUpto { get; set; }
        public string NoOfMonths { get; set; } = string.Empty; // e.g., "3 Months"
        public string JustificationGMReason { get; set; } = string.Empty;
        public double AvgRating { get; set; }
        public int ECID { get; set; } // The Confirmation ID for that extension period
    }
    // Add this class to the employee_confirmation_api_final.DTOs namespace

    public class SaveRmFeedbackDto
    {
        public int Ecid { get; set; }
        public int ParamId { get; set; }
        public int RmRating { get; set; }
        public string RmRemarks { get; set; } = string.Empty;
    }
    // Add this class to the employee_confirmation_api_final.DTOs namespace

    public class SubmitRmDecisionDto
    {
        public int Ecid { get; set; }
        public int MEmpId { get; set; } // The employee ID being confirmed
        public DateTime ConfirmDate { get; set; }
        public int IsForConfirmation { get; set; } // The decision (e.g., 1=Confirm, 0=Extend, etc.)
        public string JustificationOrReason { get; set; } = string.Empty;
        public int Extension { get; set; } // Number of months to extend, if applicable
    }
// Add these classes to the dead2.DTOs namespace

public class SaveHeadFeedbackDto
{
    public int Ecid { get; set; }
    public int ParamId { get; set; }
    public int HeadRating { get; set; }
    public string HeadRemarks { get; set; } = string.Empty;
}

public class SubmitGhDecisionDto
{
    public long InstanceId { get; set; } // Note: BIGINT in SP maps to long in C#
    public int IsForConfirmationGh { get; set; } // The decision
    public string GhJustificationOrReason { get; set; } = string.Empty;
    public int? Extension { get; set; } // Nullable INT
}
}

