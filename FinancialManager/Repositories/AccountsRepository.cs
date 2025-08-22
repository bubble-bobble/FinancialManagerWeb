using Dapper;
using FinancialManager.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinancialManager.Repositories
{
    public class AccountsRepository : IAccountsRepository
    {
        private readonly string _connectionString;

        public AccountsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("FinancialManager");
        }

        public async Task InsertAccount(AccountViewModel account)
        {
            await using var connection = new SqlConnection(_connectionString);
            const string query = @"INSERT INTO Accounts (Name, Description, Balance, AccountTypeId) 
                                   VALUES (@Name, @Description, @Balance, @AccountTypeId);
                                   SELECT SCOPE_IDENTITY();";
            _ = await connection.QuerySingleAsync<int>(query, account);
        }

        public async Task<IEnumerable<AccountViewModel>> SelectAccounts(int userId)
        {
            await using var connection = new SqlConnection(_connectionString);
            const string query = @"SELECT ACC.Id, ACC.Name, ACC.Balance, ACCT.Name AS AccountType 
                                   FROM Accounts AS ACC
	                                   INNER JOIN AccountTypes AS ACCT ON ACCT.Id = ACC.AccountTypeId
                                   WHERE ACCT.UserId = @UserId
                                   ORDER BY ACCT.Sequence";
            return await connection.QueryAsync<AccountViewModel>(query, new { userId });
        }

        public async Task<AccountViewModel> SelectAccount(int id, int userId)
        {
            await using var connection = new SqlConnection(_connectionString);
            const string query = @"SELECT ACC.Id, ACC.Name, ACC.Description, ACC.Balance, ACCT.Id AS AccountTypeId
                                   FROM Accounts AS ACC
                                       INNER JOIN AccountTypes AS ACCT ON ACCT.Id = ACC.AccountTypeId
                                   WHERE ACC.Id = @Id AND ACCT.UserId = @UserId";
            return await connection.QuerySingleOrDefaultAsync<AccountViewModel>(query, new { id, userId });
        }

        public async Task UpdateAccount(CreateAccountViewModel account)
        {
            await using var connection = new SqlConnection(_connectionString);
            const string query = @"UPDATE Accounts 
                                   SET Name = @Name, Description = @Description, Balance = @Balance, 
                                   AccountTypeId = @AccountTypeId
                                   WHERE Id = @Id";
            await connection.ExecuteAsync(query, account);
        }

        public async Task DeleteAccount(int id)
        {
            await using var connection = new SqlConnection(_connectionString);
            const string query = @"DELETE FROM Accounts WHERE Id = @Id;";
            await connection.ExecuteAsync(query, new { id });
        }
    }
}
