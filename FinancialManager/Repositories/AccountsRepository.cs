using Dapper;
using FinancialManager.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
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
            _ = await connection.QuerySingleAsync<int>(query, account);
        }

        public async Task<IEnumerable<AccountViewModel>> SelectAccounts(int userId)
        {
            using var connection = new SqlConnection(_connectionString);
            const string query = @"SELECT	ACC.Id, ACC.Name, ACC.Balance,	ACCT.Name AS AccountType 
                                   FROM Accounts AS ACC
	                                   INNER JOIN AccountTypes AS ACCT ON ACCT.Id = ACC.AccountTypeId
                                   WHERE ACCT.UserId = @UserId
                                   ORDER BY ACCT.Sequence";
            return await connection.QueryAsync<AccountViewModel>(query, new { userId });
        }
    }
}
