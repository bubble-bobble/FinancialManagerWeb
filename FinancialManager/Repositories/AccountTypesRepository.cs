using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using FinancialManager.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace FinancialManager.Repositories;

public class AccountTypesRepository : IAccountTypesRepository
{
    private readonly string _connectionString;

    public AccountTypesRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("FinancialManager");
    }

    public async Task InsertAccountType(AccountTypeViewModel accountType)
    {
        await using var connection = new SqlConnection(_connectionString);
        const string query = "INSERT INTO AccountTypes (Name, Sequence, UserId) VALUES (@Name, @Sequence, @UserId)";
        await connection.ExecuteAsync(query, accountType);
    }

    public async Task<bool> SelectIfExistAccountType(string name, int userId)
    {
        await using var connection = new SqlConnection(_connectionString);
        const string query = "SELECT * FROM AccountTypes WHERE Name = @Name AND UserId = @UserId";
        return await connection.ExecuteScalarAsync<bool>(query, new { Name = name, UserId = userId });
    }

    public async Task<IEnumerable<AccountTypeViewModel>> SelectAccountTypes(int userId)
    {
        await using var connection = new SqlConnection(_connectionString);
        const string query = "SELECT * FROM AccountTypes WHERE UserId = @UserId ORDER BY Sequence";
        return await connection.QueryAsync<AccountTypeViewModel>(query, new { UserId = userId });
    }

    public async Task<AccountTypeViewModel> SelectAccountType(int id, int userId)
    {
        await using var connection = new SqlConnection(_connectionString);
        const string query = "SELECT Id, Name, Sequence FROM AccountTypes WHERE Id = @Id AND UserId = @UserId";
        return await connection.QueryFirstOrDefaultAsync<AccountTypeViewModel>(query, new { Id = id, UserId = userId });       
    }
    
    public async Task UpdateAccountType(AccountTypeViewModel accountType)
    {
        await using var connection = new SqlConnection(_connectionString);
        const string query = "UPDATE AccountTypes SET Name = @Name WHERE Id = @Id";
        await connection.ExecuteAsync(query, accountType);
    }

    public async Task DeleteAccountType(int id)
    {
        await using var connection = new SqlConnection(_connectionString);
        const string query = "DELETE FROM AccountTypes WHERE Id = @Id";
        await connection.ExecuteAsync(query, new { Id = id });
    }
}