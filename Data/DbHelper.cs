using System.Data;
using System.Data.SqlClient;

namespace employee_confirmation_api_final.Data
{
    public class DbHelper
    {
        private readonly IConfiguration _config;

        public DbHelper(IConfiguration config)
        {
            _config = config;
        }

        // This property allows the repository to get the connection string if needed for direct ADO.NET
        public string ConnectionString => _config.GetConnectionString("DefaultConnection");

        private SqlConnection GetConnection() =>
            new SqlConnection(ConnectionString);

        public async Task<DataTable> ExecuteStoredProcedureAsync(string procName, Dictionary<string, object>? parameters = null)
        {
            using var conn = GetConnection();
            using var cmd = new SqlCommand(procName, conn) { CommandType = CommandType.StoredProcedure };

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                }
            }

            var adapter = new SqlDataAdapter(cmd);
            var table = new DataTable();
            await conn.OpenAsync();
            adapter.Fill(table);
            return table;
        }

        // NEW METHOD: For stored procedures that return a single value via an OUTPUT parameter
        public async Task<T> ExecuteStoredProcedureWithOutputAsync<T>(string procName, Dictionary<string, object> parameters, string outputParamName)
        {
            using var conn = GetConnection();
            using var cmd = new SqlCommand(procName, conn) { CommandType = CommandType.StoredProcedure };

            // Add all input parameters
            foreach (var param in parameters)
            {
                cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
            }

            // Add and configure the output parameter
            var outputParam = new SqlParameter(outputParamName, SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(outputParam);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            // Cast and return the output value
            return (T)outputParam.Value;
        }
    }
}