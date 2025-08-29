using Dapper;
using System.Data;

namespace HangFireJobsExample_Youtube.Repositorios;

public class UserRepository(IDbConnection dbConnection) : IUserRepository
{
    public void UserInsert(string nome, string email)
    {
        var sql = "INSERT INTO Usuarios (Nome, Email) VALUES (@Nome, @Email)";        
        dbConnection.Execute(sql, new { Nome = nome, Email = email });
    }
}
