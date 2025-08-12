using Dapper;
using FinancialManager.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace FinancialManager.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly string _connectionString;

        public AccountRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("FinancialManager");
        }

        public async Task InsertAccount(AccountViewModel account)
        {
            using var connection = new SqlConnection(_connectionString);
            const string query = @"INSERT INTO Accounts (Name, Description, Balance, AccountTypeId) 
                                   VALUES (@Name, @Description, @Balance, @AccountTypeId);
                                   SELECT SCOPE_IDENTITY();";
            _ = connection.QuerySingleAsync<int>(query, account);
        }
    }
}
