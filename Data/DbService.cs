using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace Carllet_Notification_Job.Data
{
    public class DbService
    {
        private readonly IDbConnection _connection;

        public DbService()
        {
            _connection = new NpgsqlConnection(Environment.GetEnvironmentVariable("poc_v2"));
        }

        public async Task<List<User>> GetUsers()
        {
            List<User> result = (await _connection.QueryAsync<User>("SELECT * FROM usuario")).ToList();

            return result;
        }
    }
}

