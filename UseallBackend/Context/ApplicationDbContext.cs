using System;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace UseallBackend.Context
{
    class ApplicationDbContext : DbContext 
    {
        static NpgsqlConnection connection;
        public static void ConnectDb() {
            var connectionString = "Host=localhost;Port=5432;Database=Useall;Username=USER_DB;Password=SENHA";
            connection = new NpgsqlConnection(connectionString);
            connection.Open();
        }
        public static NpgsqlDataReader ReadCommand(string command) {
            ConnectDb();
            var cmd = new NpgsqlCommand(command, connection);
            return cmd.ExecuteReader();
        }

        public static int RowsAffected(string command) {
            ConnectDb();
            var cmd = new NpgsqlCommand(command, connection);
            return cmd.ExecuteNonQuery();
        }
    }
}
