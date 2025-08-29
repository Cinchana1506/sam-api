using employee_confirmation_api_final.Data;
using employee_confirmation_api_final.DTOs;
using employee_confirmation_api_final.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace employee_confirmation_api_final.Repositories
{
    public class EmployeeConfirmationRepository : IEmployeeConfirmationRepository
    {
        private readonly DbHelper _db;

        public EmployeeConfirmationRepository(DbHelper db)
        {
            _db = db;
        }

        public async Task<List<EmployeeForConfirmationDto>> GetEmployeesForConfirmationAsync()
        {
            var table = await _db.ExecuteStoredProcedureAsync("Sch_EmpConfirmation_GetEmployeesForConfirmation");

            var employees = new List<EmployeeForConfirmationDto>();
            foreach (DataRow row in table.Rows)
            {
                employees.Add(new EmployeeForConfirmationDto
                {
                    EmployeeId = Convert.ToInt32(row["EmployeeId"]),
                    EmployeeName = row["EmployeeName"].ToString() ?? "",
                    Department = row["Department"].ToString() ?? "",
                    JoiningDate = Convert.ToDateTime(row["JoiningDate"])
                });
            }

            return employees;
        }

        // NEW METHOD
        public async Task<EmployeeConfirmationInsertResponseDto> InsertEmployeeConfirmationAsync(EmployeeConfirmationInsertDto dto)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@MEmpID", dto.EmpId },
                { "@MProjectID", dto.ProjectId },
                { "@ConfirmDate", dto.ConfirmDate }
            };

            using var conn = new SqlConnection(_db.ConnectionString);
            using var cmd = new SqlCommand("Sch_EmpConfirmation_InsertSchedularDetails", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            foreach (var param in parameters)
            {
                cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
            }

            var outputParam = new SqlParameter("@ECID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(outputParam);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            return new EmployeeConfirmationInsertResponseDto
            {
                ECID = (int)outputParam.Value
            };
        }
        public async Task<ConfirmationMasterDetailsResponseDto> GetConfirmationDetailsByMasterIdAsync(int ecid)
        {

            var parameters = new Dictionary<string, object> { { "@ECID", ecid } };

            // Use DataSet to handle multiple result sets
            using var conn = new SqlConnection(_db.ConnectionString);
            using var cmd = new SqlCommand("EmpConfirmation_GetDetailsByMasterID", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@ECID", ecid);

            var adapter = new SqlDataAdapter(cmd);
            var dataSet = new DataSet();

            await conn.OpenAsync();
            adapter.Fill(dataSet);

            var response = new ConfirmationMasterDetailsResponseDto();

            // Map Result Set 0: Master Details (assuming it's the first table)
            if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                var row = dataSet.Tables[0].Rows[0];
                response.MasterDetails = new ConfirmationDetailsDto
                {
                    // Map all fields from the first result set
                    MEmpId = Convert.ToInt32(row["MEmpId"]),
                    FullName = row["FullName"].ToString() ?? "",
                    EmployeeId = row["EmployeeId"].ToString() ?? "",
                    // ... Map all other fields from the first result set here ...
                    // Example:
                    // IsConfirmed = Convert.ToBoolean(row["IsConfirmed"]),
                    // ProjectName = row["ProjectName"].ToString() ?? ""
                };
            }

            // Map Result Set 1: Evaluation Parameters (assuming it's the second table)
            if (dataSet.Tables.Count > 1)
            {
                foreach (DataRow row in dataSet.Tables[1].Rows)
                {
                    response.EvaluationParameters.Add(new EvaluationParameterDto
                    {
                        ParamId = Convert.ToInt32(row["ParamID"]),
                        Category = row["Category"].ToString() ?? "",
                        Parameter = row["Parameter"].ToString() ?? "",
                        ParameterDescription = row["ParameterDescription"].ToString() ?? "",
                        RMRating = row["RMRRating"].ToString() ?? "0",
                        RMRemarks = row["RMRemarks"].ToString() ?? "",
                        HeadRating = row["HeadRating"].ToString() ?? "0",
                        HeadRemarks = row["HeadRemarks"].ToString() ?? "",
                        ParameterOrder = Convert.ToInt32(row["ParameterOrder"])
                    });
                }
            }

            // Map Result Set 2: Average Rating (assuming it's the third table)
            if (dataSet.Tables.Count > 2 && dataSet.Tables[2].Rows.Count > 0)
            {
                // Check if the value is DBNull before converting
                if (dataSet.Tables[2].Rows[0]["AvgRating"] != DBNull.Value)
                {
                    response.AverageRating = Convert.ToDecimal(dataSet.Tables[2].Rows[0]["AvgRating"]);
                }
            }

            return response;
        }




        // NEW METHOD - Gets all projects for a dropdown
        public async Task<List<ProjectDto>> GetProjectsAsync()
        {
            // Using the existing, perfect DbHelper method
            var table = await _db.ExecuteStoredProcedureAsync("Sch_GetActiveProjects");

            var projects = new List<ProjectDto>();
            foreach (DataRow row in table.Rows)
            {
                projects.Add(new ProjectDto
                {
                    ProjectId = Convert.ToInt32(row["ProjectId"]), // Assuming the column name is ProjectId
                    ProjectName = row["ProjectName"].ToString() ?? "" // Assuming the column name is ProjectName
                    // Map other fields if necessary, e.g., Convert.ToBoolean(row["IsActive"])
                });
            }
            return projects;
        }
        public async Task<List<ExtensionDetailDto>> GetExtensionDetailsAsync(int mempId, int instanceId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@MEmpID", mempId },
                { "@InstanceID", instanceId }
            };

            // Using the existing DbHelper method
            var table = await _db.ExecuteStoredProcedureAsync("EmpConfirmation_GetExtensionDetails", parameters);

            var extensionHistory = new List<ExtensionDetailDto>();
            foreach (DataRow row in table.Rows)
            {
                extensionHistory.Add(new ExtensionDetailDto
                {
                    ExtendedFrom = Convert.ToDateTime(row["ExtendedFrom"]),
                    ExtendedUpto = Convert.ToDateTime(row["ExtendedUpto"]),
                    NoOfMonths = row["NoOfMonths"].ToString() ?? "",
                    JustificationGMReason = row["JustificationGMReason"].ToString() ?? "",
                    // Handle potential DBNull for AvgRating
                    AvgRating = row["AvgRating"] == DBNull.Value ? 0.0 : Convert.ToDouble(row["AvgRating"]),
                    ECID = Convert.ToInt32(row["ECID"])
                });
            }
            return extensionHistory;
        }
        public async Task<bool> SaveRmFeedbackAsync(SaveRmFeedbackDto dto)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@ECID", dto.Ecid },
                { "@ParamID", dto.ParamId },
                { "@RMRating", dto.RmRating },   // Use @RMRating if that's the SP parameter name
                { "@RMRemarks", dto.RmRemarks }
            };

            // The SP doesn't return a result set, it just performs an insert/update.
            // We call it and assume success if no exception is thrown.
            await _db.ExecuteStoredProcedureAsync("EmpConfirmation_UpdateRMFeedback", parameters);

            return true; // Indicate success
        }
        public async Task<bool> SubmitRmDecisionAsync(SubmitRmDecisionDto dto)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@ECID", dto.Ecid },
                { "@MEmpID", dto.MEmpId },
                { "@ConfirmDate", dto.ConfirmDate },
                { "@IsForConfirmation", dto.IsForConfirmation },
                { "@JustificationORReason", dto.JustificationOrReason },
                { "@Extension", dto.Extension }
            };

            // The SP performs the update and calculates dates.
            // We assume success if no exception is thrown.
            await _db.ExecuteStoredProcedureAsync("EmpConfirmation_UpdateDetails", parameters);

            return true; // Indicate success
        }
        public async Task<bool> SaveHeadFeedbackAsync(SaveHeadFeedbackDto dto)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@ECID", dto.Ecid },
                { "@ParamID", dto.ParamId },
                { "@HeadRating", dto.HeadRating },
                { "@HeadRemarks", dto.HeadRemarks }
            };

            await _db.ExecuteStoredProcedureAsync("EmpConfirmation_UpdateHeadFeedback", parameters);
            return true;
        }

        // NEW METHOD - Submits the GH's final decision
        public async Task<bool> SubmitGhDecisionAsync(SubmitGhDecisionDto dto)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@InstanceID", dto.InstanceId },
                { "@IsForConfirmationGH", dto.IsForConfirmationGh },
                { "@GhJustificationORReason", dto.GhJustificationOrReason },
                { "@Extension", dto.Extension ?? (object)DBNull.Value } // Handle nullable int
            };

            await _db.ExecuteStoredProcedureAsync("EmployeeConfirmation_UpdateIsForConfirmationGH", parameters);
            return true;
        }
    }
}
    

